using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class EditCategoryInWishKnowledge : IJob
    {
        public const int IntervalInSeconds = 15;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var successfullJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetAddCategoryToWishKnowledge();
                var categoryUserPairs = new List<CategoryUserPair>();
                foreach (var job in jobs)
                {
                    var serializer = new JavaScriptSerializer();
                    var categoryUserIdPair = serializer.Deserialize<CategoryUserPair>(job.JobContent);
                    categoryUserPairs.Add(categoryUserIdPair);
                }
                foreach (var categoryUserPair in categoryUserPairs)
                {
                    try
                    {
                        var user = Sl.UserRepo.GetById(categoryUserPair.UserId);
                        CategoryInKnowledge.PinQuestionsInCategory(categoryUserPair.CategoryId, user);
                        CategoryInKnowledge.UpdateCategoryValuation(categoryUserPair.CategoryId, user);

                        //scope.R<ReputationUpdate>().Run(scope.R<UserRepo>().GetById(Convert.ToInt32(userJobs.Key)));
                        successfullJobIds.AddRange(jobs.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                        new RollbarClient().SendException(e);
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job EditCategoryInWishKnowledge added for "+ successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }
    }

    public class CategoryUserPair
    {
        public int CategoryId;
        public int UserId;
    }
}

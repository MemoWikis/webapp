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
        public const int IntervalInSeconds = 2;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var successfullJobIds = new List<int>();

                var allJobs = new List<JobQueue>();
                allJobs.AddRange(scope.R<JobQueueRepo>().GetAddCategoryToWishKnowledge());
                allJobs.AddRange(scope.R<JobQueueRepo>().GetRemoveQuestionsInCategoryFromWishKnowledge());
                allJobs = allJobs.OrderBy(j => j.Id).ToList();

                foreach (var job in allJobs)
                {
                    switch (job.JobQueueType)
                    {
                        case JobQueueType.AddCategoryToWishKnowledge:
                            AddCategoryToWishKnowledge(job, successfullJobIds);
                            break;

                        case JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge:
                            RemoveQuestionsInCategoryFromWishKnowledge(job, successfullJobIds);
                            break;
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job EditCategoryInWishKnowledge edited Valuations for "+ successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }

        private static void AddCategoryToWishKnowledge(JobQueue job, IList<int> successfullJobIds)
        {
                try
                {
                    var categoryUserPair = GetCategoryUserPair(job);
                    CategoryInKnowledge.PinCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);

                    successfullJobIds.Add(job.Id);
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                    new RollbarClient().SendException(e);
                }
        }

        private static void RemoveQuestionsInCategoryFromWishKnowledge(JobQueue job, List<int> successfullJobIds)
        {
                try
                {
                    var categoryUserPair = GetCategoryUserPair(job);
                    CategoryInKnowledge.UnpinQuestionsInCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);

                    successfullJobIds.Add(job.Id);
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                    new RollbarClient().SendException(e);
                }
        }

        private static CategoryUserPair GetCategoryUserPair(JobQueue jobQueueEntry)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<CategoryUserPair>(jobQueueEntry.JobContent);
        }
    }

    public class CategoryUserPair
    {
        public int CategoryId;
        public int UserId;
    }
}

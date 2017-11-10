using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Autofac;
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
                AddCategoryToWishKnowledge(scope, successfullJobIds);
                RemoveQuestionsInCategoryFromWishKnowledge(scope, successfullJobIds);

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job EditCategoryInWishKnowledge Valuations for "+ successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }

        private static void AddCategoryToWishKnowledge(ILifetimeScope scope, List<int> successfullJobIds)
        {
            var addToKnowledgeJobs = scope.R<JobQueueRepo>().GetAddCategoryToWishKnowledge();
            var categoryUserPairs = GetCategoryUserPairs(addToKnowledgeJobs);

            foreach (var categoryUserPair in categoryUserPairs)
            {
                try
                {
                    CategoryInKnowledge.PinCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);

                    successfullJobIds.AddRange(addToKnowledgeJobs.Select(j => j.Id).ToList());
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                    new RollbarClient().SendException(e);
                }
            }
        }

        private static void RemoveQuestionsInCategoryFromWishKnowledge(ILifetimeScope scope, List<int> successfullJobIds)
        {
            var removeFromKnowledgeJobs = scope.R<JobQueueRepo>().GetRemoveQuestionsInCategoryFromWishKnowledge();
            var categoryUserPairs = GetCategoryUserPairs(removeFromKnowledgeJobs);

            foreach (var categoryUserPair in categoryUserPairs)
            {
                try
                {
                    CategoryInKnowledge.UnpinQuestionsInCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);

                    successfullJobIds.AddRange(removeFromKnowledgeJobs.Select(j => j.Id).ToList());
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                    new RollbarClient().SendException(e);
                }
            }
        }

        private static List<CategoryUserPair> GetCategoryUserPairs(IList<JobQueue> jobQueueEntries)
        {
            var categoryUserPairs = new List<CategoryUserPair>();
            foreach (var job in jobQueueEntries)
            {
                var serializer = new JavaScriptSerializer();
                categoryUserPairs.Add(serializer.Deserialize<CategoryUserPair>(job.JobContent));
            }
            return categoryUserPairs;
        }
    }

    public class CategoryUserPair
    {
        public int CategoryId;
        public int UserId;
    }
}

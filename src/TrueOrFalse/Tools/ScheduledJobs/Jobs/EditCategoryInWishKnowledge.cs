using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Autofac;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class EditCategoryInWishKnowledge : IJob
    {
        private readonly SessionUser _sessionUser;
        private readonly CategoryInKnowledge _categoryInKnowledge;

        public const int IntervalInSeconds = 2;

        public EditCategoryInWishKnowledge(SessionUser sessionUser, CategoryInKnowledge categoryInKnowledge )
        {
            _sessionUser = sessionUser;
            _categoryInKnowledge = categoryInKnowledge;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var allJobs = new List<JobQueue>();
                allJobs.AddRange(scope.Resolve<JobQueueRepo>().GetRemoveQuestionsInCategoryFromWishKnowledge());
                allJobs = allJobs.OrderBy(j => j.Id).ToList();

                foreach (var job in allJobs)
                {
                    switch (job.JobQueueType)
                    {

                        case JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge:
                            RemoveQuestionsInCategoryFromWishKnowledge(job, scope, _sessionUser);
                            break;
                    }
                }
            }, "EditCategoryInWishKnowledge");
        }

        private void RemoveQuestionsInCategoryFromWishKnowledge(JobQueue job, ILifetimeScope scope,SessionUser sessionUser)
        {
            var categoryUserPair = new CategoryUserPair();
            try
            { 
                categoryUserPair = GetCategoryUserPair(job);
                _categoryInKnowledge.UnpinQuestionsInCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId, sessionUser);
                
                scope.Resolve<JobQueueRepo>().Delete(job.Id);
                Logg.r().Information($"Job EditCategoryInWishKnowledge removed QuestionValuations for Category { categoryUserPair.CategoryId } and User { categoryUserPair.UserId }");
            }
            catch (Exception e)
            {

                Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge. {Method} {CategoryId}", "RemoveQuestionsInCategoryFromWishKnowledge", categoryUserPair.CategoryId);
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

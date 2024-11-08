using Autofac;
using Newtonsoft.Json;
using Quartz;
using Rollbar;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class EditCategoryInWishKnowledge : IJob
    {
        private readonly PageInKnowledge _pageInKnowledge;

        public const int IntervalInSeconds = 2;

        public EditCategoryInWishKnowledge(PageInKnowledge pageInKnowledge)
        {
            _pageInKnowledge = pageInKnowledge;
        }
        public async Task Execute(IJobExecutionContext context)
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
                            RemoveQuestionsInCategoryFromWishKnowledge(job, scope);
                            break;
                    }
                }
            }, "EditCategoryInWishKnowledge");
        }

        private void RemoveQuestionsInCategoryFromWishKnowledge(JobQueue job, ILifetimeScope scope)
        {
            var categoryUserPair = new CategoryUserPair();
            try
            { 
                categoryUserPair = GetCategoryUserPair(job);
                _pageInKnowledge.UnpinQuestionsInCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);
                
                scope.Resolve<JobQueueRepo>().Delete(job.Id);
                Logg.r.Information($"Job EditCategoryInWishKnowledge removed QuestionValuations for Category { categoryUserPair.CategoryId } and User { categoryUserPair.UserId }");
            }
            catch (Exception e)
            {

                Logg.r.Error(e, "Error in job EditCategoryInWishKnowledge. {Method} {CategoryId}", "RemoveQuestionsInCategoryFromWishKnowledge", categoryUserPair.CategoryId);
                RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
            }
        }

        private static CategoryUserPair? GetCategoryUserPair(JobQueue jobQueueEntry)
        {
            return JsonConvert.DeserializeObject<CategoryUserPair>(jobQueueEntry.JobContent);
        }
    }

    public class CategoryUserPair
    {
        public int CategoryId;
        public int UserId;
    }
}

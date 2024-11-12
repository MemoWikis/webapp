using Autofac;
using Newtonsoft.Json;
using Quartz;
using Rollbar;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class EditPageInWishKnowledge : IJob
    {
        private readonly PageInKnowledge _pageInKnowledge;

        public const int IntervalInSeconds = 2;

        public EditPageInWishKnowledge(PageInKnowledge pageInKnowledge)
        {
            _pageInKnowledge = pageInKnowledge;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var allJobs = new List<JobQueue>();
                allJobs.AddRange(scope.Resolve<JobQueueRepo>().GetRemoveQuestionsInPageFromWishKnowledge());
                allJobs = allJobs.OrderBy(j => j.Id).ToList();

                foreach (var job in allJobs)
                {
                    switch (job.JobQueueType)
                    {

                        case JobQueueType.RemoveQuestionsInPageFromWishKnowledge:
                            RemoveQuestionsInPageFromWishKnowledge(job, scope);
                            break;
                    }
                }
            }, "EditPageInWishKnowledge");
        }

        private void RemoveQuestionsInPageFromWishKnowledge(JobQueue job, ILifetimeScope scope)
        {
            var pageUserPair = new PageUserPair();
            try
            {
                pageUserPair = GetPageUserPair(job);
                _pageInKnowledge.UnpinQuestionsInPageInDatabase(pageUserPair.PageId, pageUserPair.UserId);

                scope.Resolve<JobQueueRepo>().Delete(job.Id);
                Logg.r.Information($"Job EditPageInWishKnowledge removed QuestionValuations for Page {pageUserPair.PageId} and User {pageUserPair.UserId}");
            }
            catch (Exception e)
            {

                Logg.r.Error(e, "Error in job EditPageInWishKnowledge. {Method} {PageId}", "RemoveQuestionsInPageFromWishKnowledge", pageUserPair.PageId);
                RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
            }
        }

        private static PageUserPair? GetPageUserPair(JobQueue jobQueueEntry)
        {
            return JsonConvert.DeserializeObject<PageUserPair>(jobQueueEntry.JobContent);
        }
    }

    public class PageUserPair
    {
        public int PageId;
        public int UserId;
    }
}

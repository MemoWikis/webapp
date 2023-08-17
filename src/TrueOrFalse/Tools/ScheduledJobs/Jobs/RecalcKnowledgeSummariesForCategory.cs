using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;
using Rollbar;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeSummariesForCategory : IJob
    {
        private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
        private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
        private readonly CategoryValuationWritingRepo _categoryValuationWritingRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecalcKnowledgeSummariesForCategory(CategoryValuationReadingRepo categoryValuationReadingRepo,
            KnowledgeSummaryLoader knowledgeSummaryLoader,
            CategoryValuationWritingRepo categoryValuationWritingRepo,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _categoryValuationReadingRepo = categoryValuationReadingRepo;
            _knowledgeSummaryLoader = knowledgeSummaryLoader;
            _categoryValuationWritingRepo = categoryValuationWritingRepo;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                List<int> successfullJobIds = new List<int>();
                var jobs = scope.Resolve<JobQueueRepo>().GetRecalcKnowledgeSummariesForCategory();
                var jobsByCategoryId = jobs.GroupBy(j => j.JobContent);
                foreach (var grouping in jobsByCategoryId)
                {
                    try
                    {
                        KnowledgeSummaryUpdate.RunForCategory(Convert.ToInt32(grouping.Key), 
                            _categoryValuationReadingRepo, 
                            _categoryValuationWritingRepo,
                            _knowledgeSummaryLoader);

                        successfullJobIds.AddRange(grouping.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, "Error in job RecalcKnowledgeSummaryForCategory.");
                        RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.Resolve<JobQueueRepo>().DeleteById(successfullJobIds);
                    new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job RecalcKnowledgeSummaryForCategory recalculated knowledge summary for " + successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }
            }, "RecalcKnowledgeSummaryForCategory");
            return Task.CompletedTask; 
        }
    }
}
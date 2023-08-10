using Microsoft.AspNetCore.Http;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionReadingRepo _questionReadingRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LomExportJob(CategoryRepository categoryRepository, 
            QuestionReadingRepo questionReadingRepo, 
            IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _questionReadingRepo = questionReadingRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
                LomExporter.AllToFileSystem(_categoryRepository, 
                    _questionReadingRepo, 
                    _httpContextAccessor), 
                nameof(LomExportJob));

            return Task.CompletedTask;
        }
    }
}
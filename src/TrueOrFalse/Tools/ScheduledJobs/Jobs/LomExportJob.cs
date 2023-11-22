using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionReadingRepo _questionReadingRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IActionContextAccessor _actionContextAccessor;

        public LomExportJob(CategoryRepository categoryRepository, 
            QuestionReadingRepo questionReadingRepo, 
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            IActionContextAccessor actionContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _questionReadingRepo = questionReadingRepo;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _actionContextAccessor = actionContextAccessor;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
                new LomExporter(_categoryRepository,
                    _questionReadingRepo, 
                    _httpContextAccessor, 
                    _webHostEnvironment, 
                    _actionContextAccessor)
                    .AllToFileSystem(), 
                nameof(LomExportJob));

            return Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
        private readonly UserReadingRepo _userReadingRepo;
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InitUserValuationCache(CategoryValuationReadingRepo categoryValuationReadingRepo,
            UserReadingRepo userReadingRepo, 
            QuestionValuationRepo questionValuationRepo,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _categoryValuationReadingRepo = categoryValuationReadingRepo;
            _userReadingRepo = userReadingRepo;
            _questionValuationRepo = questionValuationRepo;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                SessionUserCache.CreateItemFromDatabase(dataMap.GetInt("userId"), _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo);

            }, "InitUserValuationCache");

            return Task.CompletedTask;
        }
    }
}


using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
        private readonly UserReadingRepo _userReadingRepo;
        private readonly QuestionValuationRepo _questionValuationRepo;

        public InitUserValuationCache(CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
        {
            _categoryValuationReadingRepo = categoryValuationReadingRepo;
            _userReadingRepo = userReadingRepo;
            _questionValuationRepo = questionValuationRepo;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                SessionUserCache.CreateItemFromDatabase(dataMap.GetInt("userId"), _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo);

            }, "InitUserValuationCache");

            return Task.CompletedTask;
        }
    }
}

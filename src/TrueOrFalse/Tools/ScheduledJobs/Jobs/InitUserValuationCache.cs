
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
        private readonly UserRepo _userRepo;
        private readonly QuestionValuationRepo _questionValuationRepo;

        public InitUserValuationCache(CategoryValuationReadingRepo categoryValuationReadingRepo, UserRepo userRepo, QuestionValuationRepo questionValuationRepo)
        {
            _categoryValuationReadingRepo = categoryValuationReadingRepo;
            _userRepo = userRepo;
            _questionValuationRepo = questionValuationRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                SessionUserCache.CreateItemFromDatabase(dataMap.GetInt("userId"), _categoryValuationReadingRepo, _userRepo, _questionValuationRepo);

            }, "InitUserValuationCache");
        }
    }
}


using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly CategoryValuationRepo _categoryValuationRepo;
        private readonly UserRepo _userRepo;
        private readonly QuestionValuationRepo _questionValuationRepo;

        public InitUserValuationCache(CategoryValuationRepo categoryValuationRepo, UserRepo userRepo, QuestionValuationRepo questionValuationRepo)
        {
            _categoryValuationRepo = categoryValuationRepo;
            _userRepo = userRepo;
            _questionValuationRepo = questionValuationRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                SessionUserCache.CreateItemFromDatabase(dataMap.GetInt("userId"), _categoryValuationRepo, _userRepo, _questionValuationRepo);

            }, "InitUserValuationCache");
        }
    }
}

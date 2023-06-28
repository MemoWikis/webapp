
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly CategoryValuationRepo _categoryValuationRepo;

        public InitUserValuationCache(CategoryValuationRepo categoryValuationRepo)
        {
            _categoryValuationRepo = categoryValuationRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                SessionUserCache.CreateItemFromDatabase(dataMap.GetInt("userId"), _categoryValuationRepo);

            }, "InitUserValuationCache");
        }
    }
}

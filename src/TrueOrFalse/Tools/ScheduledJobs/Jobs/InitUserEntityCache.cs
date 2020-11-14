
using Quartz;

namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class InitUserEntityCache : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information(" InitUserEntityCache started");

                UserEntityCache.Init();

            }, "InitUserEntityCache");
        }
    }
}

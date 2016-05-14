using System.Threading;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(1000);
            }, "TestJob");
        }
    }
}
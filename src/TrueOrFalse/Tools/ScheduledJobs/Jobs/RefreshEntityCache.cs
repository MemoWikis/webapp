using System.Threading.Tasks;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RefreshEntityCache : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                EntityCache.Init(" (in JobScheduler) ");
            }, "RefreshEntityCache");

            return Task.CompletedTask;
        }

    }
}
using System.Threading.Tasks;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => LomExporter.AllToFileSystem(), nameof(LomExportJob));
            return Task.CompletedTask;
        }
    }
}
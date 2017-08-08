using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => LomExporter.AllToFileSystem(), nameof(LomExportJob));
        }
    }
}
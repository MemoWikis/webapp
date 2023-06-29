using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        private readonly CategoryRepository _categoryRepository;

        public LomExportJob(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => LomExporter.AllToFileSystem(_categoryRepository), nameof(LomExportJob));
        }
    }
}
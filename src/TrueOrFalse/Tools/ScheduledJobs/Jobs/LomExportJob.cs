using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionRepo _questionRepo;

        public LomExportJob(CategoryRepository categoryRepository, QuestionRepo questionRepo)
        {
            _categoryRepository = categoryRepository;
            _questionRepo = questionRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => LomExporter.AllToFileSystem(_categoryRepository, _questionRepo), nameof(LomExportJob));
        }
    }
}
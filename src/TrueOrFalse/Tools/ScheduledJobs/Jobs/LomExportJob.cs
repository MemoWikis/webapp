using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class LomExportJob : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionReadingRepo _questionReadingRepo;

        public LomExportJob(CategoryRepository categoryRepository, QuestionReadingRepo questionReadingRepo)
        {
            _categoryRepository = categoryRepository;
            _questionReadingRepo = questionReadingRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => LomExporter.AllToFileSystem(_categoryRepository, _questionReadingRepo), nameof(LomExportJob));
        }
    }
}
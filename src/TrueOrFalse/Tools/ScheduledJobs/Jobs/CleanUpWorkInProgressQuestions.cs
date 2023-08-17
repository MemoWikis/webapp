using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class CleanUpWorkInProgressQuestions : IJob
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CleanUpWorkInProgressQuestions(IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var questions = scope.Resolve<ISession>().QueryOver<Question>()
                    .Where(q => q.IsWorkInProgress && q.DateCreated < DateTime.Now.AddHours(-6))
                    .List<Question>();

                var questionWritingRepo = scope.Resolve<QuestionWritingRepo>();

                foreach (var question in questions)
                    questionWritingRepo.Delete(question);

                new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("CleanUpWorkInProgressQuestions: {amountOfDeletedQuestions}", questions.Count);
            }, "CleanUpWorkInProgressQuestions");

            return Task.CompletedTask;
        }
    }
}
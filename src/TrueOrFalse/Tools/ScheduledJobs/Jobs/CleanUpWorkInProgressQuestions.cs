using Autofac;
using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class CleanUpWorkInProgressQuestions : IJob
    {
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

                Logg.r.Information("CleanUpWorkInProgressQuestions: {amountOfDeletedQuestions}", questions.Count);
            }, "CleanUpWorkInProgressQuestions");

            return Task.CompletedTask;
        }
    }
}
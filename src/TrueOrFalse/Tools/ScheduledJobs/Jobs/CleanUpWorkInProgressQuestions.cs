using System;
using Autofac;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class CleanUpWorkInProgressQuestions : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var questions = scope.Resolve<ISession>().QueryOver<Question>()
                    .Where(q => q.IsWorkInProgress && q.DateCreated < DateTime.Now.AddHours(-6))
                    .List<Question>();

                var questionRepo = scope.Resolve<QuestionRepo>();

                foreach (var question in questions)
                    questionRepo.Delete(question);

                Logg.r().Information("CleanUpWorkInProgressQuestions: {amountOfDeletedQuestions}", questions.Count);
            }, "CleanUpWorkInProgressQuestions");
        }
    }
}
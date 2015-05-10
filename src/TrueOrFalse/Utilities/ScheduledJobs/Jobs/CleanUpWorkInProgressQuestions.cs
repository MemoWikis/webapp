using System;
using Autofac;
using NHibernate;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class CleanUpWorkInProgressQuestions : IJob, IRegisterAsInstancePerLifetime
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logg.r().Information("Job start: {Job}", "CleanUpWorkInProgressQuestions ");

                using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
                {
                    var questions = scope.Resolve<ISession>().QueryOver<Question>()
                        .Where(q => q.IsWorkInProgress && q.DateCreated < DateTime.Now.AddHours(-6))
                        .List<Question>();

                    var questionRepo = scope.Resolve<QuestionRepository>();

                    foreach (var question in questions)
                        questionRepo.Delete(question);

                    Logg.r().Information("Job end: {Job} {amountOfDeletedQuestions}", "CleanUpWorkInProgressQuestions", questions.Count);
                }

                
            }
            catch(Exception e)
            {
                Logg.r().Error(e, "Job error");
                (new RollbarClient()).SendException(e);
            }
        }
    }
}
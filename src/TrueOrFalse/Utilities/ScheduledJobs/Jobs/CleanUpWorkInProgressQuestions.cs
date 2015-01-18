using System;
using Autofac;
using NHibernate;
using Quartz;
using RollbarSharp;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class CleanUpWorkInProgressQuestions : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ServiceLocator.Init(AutofacWebInitializer.Run());

            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<AutofacCoreModule>();

                Logg.r().Information("Job start: {Job}", "CleanUpWorkInProgressQuestions ");

                var questions = Sl.R<ISession>().QueryOver<Question>()
                    .Where(q => q.IsWorkInProgress && q.DateCreated < DateTime.Now.AddHours(-6))
                    .List<Question>();

                var questionRepo = Sl.R<QuestionRepository>();

                foreach (var question in questions)
                    questionRepo.Delete(question);

                Logg.r().Information("Job end: {Job} {amountOfDeletedQuestions}", "CleanUpWorkInProgressQuestions", questions.Count);
            }
            catch(Exception e)
            {
                Logg.r().Error(e, "Job error");
                (new RollbarClient()).SendException(e);
            }
        }
    }
}
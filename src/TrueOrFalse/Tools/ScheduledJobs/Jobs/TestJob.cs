using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TestJob1 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(10);
                Logg.r().Information("Hashcode ISession {0}", scope.R<ISession>().GetHashCode());
            }, "TestJob1");

            return Task.CompletedTask;
        }
    }

    public class TestJob2 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(10);
                Logg.r().Information("Hashcode ISession {0}", scope.R<ISession>().GetHashCode());
            }, "TestJob2");

            return Task.CompletedTask;
        }
    }
}
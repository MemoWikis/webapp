using System.Threading;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TestJob1 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(10);
                Logg.r().Information("Hashcode ISession {0}", scope.R<ISession>().GetHashCode());
            }, "TestJob1");
        }
    }

    public class TestJob2 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(10);
                Logg.r().Information("Hashcode ISession {0}", scope.R<ISession>().GetHashCode());
            }, "TestJob2");
        }
    }
}
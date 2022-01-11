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
                Thread.Sleep(1);
                Logg.r().Information("HttpContext {0}", System.Web.HttpContext.Current);
            }, "TestJob1");
        }
    }

    public class TestJob2 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(1);
                Logg.r().Information("HttpContext {0}", System.Web.HttpContext.Current);
            }, "TestJob2");
        }
    }
}
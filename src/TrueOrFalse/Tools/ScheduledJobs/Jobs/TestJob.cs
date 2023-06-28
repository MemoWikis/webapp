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
    public class TestJobCacheInitializer : IJob
    {
        private readonly EntityCacheInitializer _entityCacheInitializer;

        public TestJobCacheInitializer(EntityCacheInitializer entityCacheInitializer)
        {
            _entityCacheInitializer = entityCacheInitializer;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                _entityCacheInitializer.Init(" (in JobScheduler) ");
            }, "RefreshEntityCache");
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
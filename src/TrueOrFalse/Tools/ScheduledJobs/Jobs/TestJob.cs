using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TestJob1 : IJob
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TestJob1(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(1);
                new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("HttpContext {0}", _httpContextAccessor.HttpContext);
            }, "TestJob1");

            return Task.CompletedTask;
        }
    }
    public class TestJobCacheInitializer : IJob
    {
        private readonly EntityCacheInitializer _entityCacheInitializer;

        public TestJobCacheInitializer(EntityCacheInitializer entityCacheInitializer)
        {
            _entityCacheInitializer = entityCacheInitializer;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                _entityCacheInitializer.Init(" (in JobScheduler) ");
            }, "RefreshEntityCache");

            return Task.CompletedTask;
        }
    }


    public class TestJob2 : IJob
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TestJob2(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Thread.Sleep(1);
                new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("HttpContext {0}", _httpContextAccessor);
            }, "TestJob2");

            return Task.CompletedTask;
        }
    }
}
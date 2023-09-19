using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;
using Quartz.Spi;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AutofacJobFactory : IJobFactory
    {
        private readonly IContainer _container;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AutofacJobFactory(IContainer container,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _container = container;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            ServiceLocator.Init(container);
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)_container.Resolve(bundle.JobDetail.JobType);
            }
            catch (Exception e)
            {
                Logg.r.Error(e, "Error starting Job");
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
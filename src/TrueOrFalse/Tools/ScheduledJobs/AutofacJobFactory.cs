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

        public AutofacJobFactory(IContainer container)
        {
            _container = container;
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
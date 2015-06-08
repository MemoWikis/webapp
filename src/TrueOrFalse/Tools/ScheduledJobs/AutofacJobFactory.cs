using System;
using Autofac;
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
            return (IJob)_container.Resolve(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
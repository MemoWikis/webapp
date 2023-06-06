﻿using System.Diagnostics;
using System.Linq;
using Autofac;
using NHibernate.Util;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RefreshEntityCache : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                EntityCache.Init(" (in JobScheduler) ");
            }, "RefreshEntityCache");
        }

    }
}
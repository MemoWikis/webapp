using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                UserValuationCache.GetItem(dataMap.GetInt("userId"));

                Thread.Sleep(3000);

            }, "InitUserValuationCache");
        }

    }
}

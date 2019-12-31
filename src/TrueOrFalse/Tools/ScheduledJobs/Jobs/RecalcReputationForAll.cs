using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcReputationForAll : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                scope.R<JobQueueRepo>().DeleteAllJobs(JobQueueType.UpdateReputationForUser);
                scope.R<ReputationUpdate>().RunForAll();
            }, "RecalcReputationForAll");
        }

    }
}
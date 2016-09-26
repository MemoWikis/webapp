using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    //[DisallowConcurrentExecution]
    public class RecalcReputation : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                List<int> successfullJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetReputationUpdateUsers();
                var jobsByUserId = jobs.GroupBy(j => j.JobContent);
                foreach (var userJobs in jobsByUserId)
                {
                    try
                    {
                        scope.R<ReputationUpdate>().Run(scope.R<UserRepo>().GetById(Convert.ToInt32(userJobs.First().JobContent)));
                        successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        Logg.r().Error(e, "Error in job RecalcReputation");
                        new RollbarClient().SendException(e);
                    }
                }

                //Delete jobs that have been executed successfully

                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }

    }
}

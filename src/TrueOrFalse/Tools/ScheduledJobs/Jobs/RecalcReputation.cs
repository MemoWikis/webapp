﻿using System.Collections.Generic;
using System.Linq;
using Autofac;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcReputation : IJob
    {
        public const int IntervalInSeconds = 2;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                List<int> successfullJobIds = new List<int>();
                var jobs = scope.Resolve<JobQueueRepo>().GetReputationUpdateUsers();
                var jobsByUserId = jobs.GroupBy(j => j.JobContent);
                
                    foreach (var userJobs in jobsByUserId)
                    {
                        if (Convert.ToInt32(userJobs.Key) == -1)
                        {
                            successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                            continue;
                        }

                        try
                        {
                            scope.Resolve<UserWritingRepo>().ReputationUpdate(scope.Resolve<UserReadingRepo>().GetById(Convert.ToInt32(userJobs.Key)));
                            successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                        }
                        catch (Exception e)
                        {
                            Logg.r().Error(e, "Error in job RecalcReputation.");
                            new RollbarClient().SendException(e);
                        }
                    }
                    
                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.Resolve<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job RecalcReputation recalculated reputation for "+ successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }
    }
}

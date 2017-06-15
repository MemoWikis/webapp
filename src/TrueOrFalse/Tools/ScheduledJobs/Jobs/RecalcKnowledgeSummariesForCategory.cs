using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeSummariesForCategory : IJob
    {
        public const int IntervalInSeconds = 5;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                List<int> successfullJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetRecalcKnowledgeSummariesForCategory();
                var jobsByCategoryId = jobs.GroupBy(j => j.JobContent);
                foreach (var grouping in jobsByCategoryId)
                {
                    try
                    {
                        KnowledgeSummaryUpdate.RunForCategory(Convert.ToInt32(grouping.Key));
                        successfullJobIds.AddRange(grouping.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        Logg.r().Error(e, "Error in job RecalcKnowledgeSummaryForCategory.");
                        new RollbarClient().SendException(e);
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job RecalcKnowledgeSummaryForCategory recalculated knowledge summary for " + successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }
            }, "RecalcKnowledgeSummaryForCategory");
        }
    }
}
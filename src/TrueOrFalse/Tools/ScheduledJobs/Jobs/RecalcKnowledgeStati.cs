using System;
using System.Diagnostics;
using Autofac;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                var stopwatch = Stopwatch.StartNew();
                Logg.r().Information("Job start: {Job}", "RecalcKnowledgeStati ");

                foreach (var user in scope.Resolve<UserRepo>().GetAll())
                    scope.Resolve<ProbabilityUpdate_Valuation>().Run(user.Id);

                Logg.r().Information("Job end: {Job} {timeNeeded}", "RecalcKnowledgeStati", stopwatch.Elapsed);

                stopwatch.Stop();
            });
        }
    }
}
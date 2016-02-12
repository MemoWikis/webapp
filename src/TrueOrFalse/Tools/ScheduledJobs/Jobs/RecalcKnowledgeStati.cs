using System;
using System.Diagnostics;
using Autofac;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob, IRegisterAsInstancePerLifetime
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Settings.UseWebConfig = true;

                var stopwatch = Stopwatch.StartNew();
                Logg.r().Information("Job start: {Job}", "RecalcKnowledgeStati ");

                using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
                {
                    foreach (var user in scope.Resolve<UserRepo>().GetAll())
                        scope.Resolve<ProbabilityUpdate_Valuation>().Run(user.Id);

                    Logg.r().Information("Job end: {Job} {timeNeeded}", "RecalcKnowledgeStati", stopwatch.Elapsed);
                }

                stopwatch.Stop();
            }
            catch(Exception e)
            {
                Logg.r().Error(e, "Job error");
                new RollbarClient().SendException(e);
            }
        }
    }
}
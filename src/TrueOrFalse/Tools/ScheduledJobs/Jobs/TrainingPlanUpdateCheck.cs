using System;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TrainingPlanUpdateCheck : IJob
    {
        public const int IntervalInMinutes = 30;
        public static bool IsRunning = false;
        public string JobHashCode;

        public void Execute(IJobExecutionContext context)
        {
            JobHashCode = DateTime.Now.ToString().GetHashCode().ToString("x");

            JobExecute.Run(scope =>
            {
                if (IsRunning)
                {
                    Logg.r().Information("TrainingPlanUpdateCheck not executed because already running");
                    return;
                }

                IsRunning = true;

                var trainingPlans = scope.R<TrainingPlanRepo>().AllWithNewMissedDates();

                try
                {
                    foreach (var trainingPlan in trainingPlans)
                    {
                        var session = scope.R<ISession>();

                        Logg.r().Information("Updating training plan Id=" + trainingPlan.Id + " because of missed date.");

                        TrainingPlanUpdater.Run(trainingPlan);

                        session.Update(trainingPlan);
                        session.Flush();
                    }
                }
                catch (Exception e)
                {
                    Logg.r().Information(e, "");
                }

                IsRunning = false;

            }, "TrainingPlanUpdateCheck " + JobHashCode);
        }
    }
}
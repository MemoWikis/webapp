using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TrainingPlanUpdateCheck : IJob
    {
        public const int IntervalInMinutes = 30;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("Training Plan Update Check started.");
                
                var trainingPlans = scope.R<TrainingPlanRepo>().AllWithNewMissedDates();

                foreach (var trainingPlan in trainingPlans)
                {
                    TrainingPlanUpdater.Run(trainingPlan);

                    var session = scope.R<ISession>();
                    session.Update(trainingPlan);
                    session.Flush();
                }
            }, "TrainingPlanUpdateCheck");
        }
    }
}
using System.Threading;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    [DisallowConcurrentExecution]
    public class TrainingPlanUpdateCheck : IJob
    {
        public const int IntervalInMinutes = 2;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {

                var trainingPlans = scope.R<TrainingPlanRepo>().AllWithNewMissedDates();

                if (trainingPlans.Count == 0)
                {
                    Thread.Sleep(60000);
                }

                foreach (var trainingPlan in trainingPlans)
                {
                    var session = scope.R<ISession>();

                    Logg.r().Information("Updating training plan Id=" + trainingPlan.Id + " because of missed date.");

                    TrainingPlanUpdater.Run(trainingPlan);

                    session.Flush();
                }

            }, "TrainingPlanUpdateCheck");
        }
    }
}
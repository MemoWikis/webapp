using System.Linq;
using System.Threading;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    [DisallowConcurrentExecution]
    public class TrainingPlanUpdateCheck : IJob
    {
        public const int IntervalInMinutes = 30;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var trainingPlanRepo = scope.R<TrainingPlanRepo>();

                var trainingPlans = trainingPlanRepo
                    .AllWithNewMissedDates()
                    .Union(trainingPlanRepo.AllWithExpiredUncompletedDates());

                //if (trainingPlans.Count == 0)
                //{
                //    Thread.Sleep(60000);
                //}

                foreach (var trainingPlan in trainingPlans)
                {
                    var session = scope.R<ISession>();

                    Logg.r().Information("Updating training plan Id=" + trainingPlan.Id + " because of missed or uncompleted date.");

                    TrainingPlanUpdater.Run(trainingPlan);

                    session.Flush();
                }

            }, "TrainingPlanUpdateCheck");
        }
    }
}
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TrainingReminderCheck : IJob
    {
        public const int IntervalInMinutes = 3;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                var trainingDates = scope.R<TrainingDateRepo>().AllDue_InLessThen7Minutes_NotNotified();

                foreach (var trainingDate in trainingDates)
                {
                    TrainingReminderMsg.SendHtmlMail(trainingDate);
                    Logg.r().Information("Send training notification to: " + trainingDate.UserEmail());

                    trainingDate.NotificationStatus = NotificationStatus.ReminderSend;
                    scope.R<ISession>().Update(trainingDate);
                }
            }, "TrainingReminderCheck");
        }
    }
}
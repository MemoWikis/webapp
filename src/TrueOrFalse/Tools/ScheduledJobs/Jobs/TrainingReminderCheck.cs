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
                    TrainingReminderForDateMsg.SendHtmlMail(trainingDate);
                    Logg.r().Information("Send training notification to: " + trainingDate.UserEmail());

                    trainingDate.NotificationStatus = NotificationStatus.ReminderSent;
                    var session = scope.R<ISession>();
                    session.Update(trainingDate);
                    session.Flush();
                }
            }, "TrainingReminderCheck");
        }
    }
}
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class TrainingReminderCheck : IJob
    {
        public const int IntervalInMinutes = 3;

        public void Execute(IJobExecutionContext context)
        {
            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
            {
                var trainingDates = scope.R<TrainingDateRepo>().AllDue_InLessThen7Minutes_NotNotified();

                foreach (var trainingDate in trainingDates)
                {
                    TrainingReminderMsg.SendHtmlMail(trainingDate);
                    Logg.r().Information("Send training notification to: " + trainingDate.UserEmail());
                }
                    
            }
        }
    }
}
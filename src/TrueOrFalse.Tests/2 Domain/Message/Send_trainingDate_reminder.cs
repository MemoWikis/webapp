using NUnit.Framework;

[TestFixture]
public class Send_trainingDate_reminder
{
    [Test]
    public void ShouldSend()
    {
        var user = new User {EmailAddress = "test@test.de"};
        var date = new Date {User = user};
        var trainingPlan = new TrainingPlan {Date = date};
        var trainingDate = new TrainingDate {TrainingPlan = trainingPlan};
        TrainingReminderMsg.Send(trainingDate);
    }
}
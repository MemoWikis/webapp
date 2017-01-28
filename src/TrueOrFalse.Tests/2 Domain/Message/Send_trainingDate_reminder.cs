using NUnit.Framework;

[TestFixture]
public class Send_trainingDate_reminder : BaseTest
{
    [Test]
    public void ShouldSend()
    {
        var sets = ContextSet.New().AddSet("Set", numberOfQuestions: 10).All;

        var user = new User {EmailAddress = "test@test.de", Name = "Firstname Lastname"};
        var date = new Date {User = user, Details = "Test Termin", Sets = sets };
        var trainingPlan = new TrainingPlan {Date = date};
        var trainingDate = new TrainingDate {TrainingPlan = trainingPlan};
        TrainingReminderForDateMsg.SendHtmlMail(trainingDate);
    }
}
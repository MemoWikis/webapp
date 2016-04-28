using NUnit.Framework;
using TrueOrFalse.Utilities.ScheduledJobs;


[TestFixture]
public class TrainingPlanUpdateCheckJobTest : BaseTest
{
    [Test]
    public void Test()
    {
        new TrainingPlanUpdateCheck().Execute(null);
    }
}


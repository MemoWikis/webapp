using NUnit.Framework;

public class LearningSession_persistence : BaseTest
{
    [Test]
    public void Should_persist_for_set()
    {
        var user = ContextUser.GetUser();
        var set = ContextSet.New().AddSet("Setname", numberOfQuestions: 5).Persist().All[0];

        var learningSession = new LearningSession{
            User = user,
            Steps = GetLearningSessionSteps.Run(set)
        };

        R<LearningSessionRepo>().Create(learningSession);

        RecycleContainer();

        var learningSessionFromDb = R<LearningSessionRepo>().GetById(learningSession.Id);
        Assert.That(learningSessionFromDb.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void Should_persist_for_date()
    {
        var user = ContextUser.GetUser();
        var sets = ContextSet.New()
            .AddSet("Set1", numberOfQuestions: 5)
            .AddSet("Set2", numberOfQuestions: 5)
            .Persist().All;

        var date = ContextDate.New().Add(sets).Persist().All[0];
    }
}

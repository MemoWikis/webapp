using NUnit.Framework;

public class LearningSession_persistence : BaseTest
{
    [Test]
    public void Should_persist()
    {
        var user = ContextUser.GetUser();
        var set = ContextSet.New().AddSet("Setname", numberOfQuestions: 5).Persist().All[0];

        Sl.R<SessionUser>().Login(user);

        var learningSession = new LearningSession
        {
            User = user,
            Steps = GetLearningSessionSteps.Run(set)
        };

        R<LearningSessionRepo>().Create(learningSession);

        RecycleContainer();

        var learningSessionFromDb = R<LearningSessionRepo>().GetById(learningSession.Id);
        Assert.That(learningSessionFromDb.Steps.Count, Is.EqualTo(5));
    }
}

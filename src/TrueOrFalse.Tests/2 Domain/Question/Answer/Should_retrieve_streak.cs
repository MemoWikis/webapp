using System;
using NHibernate;
using NUnit.Framework;

public class Should_retrieve_streak : BaseTest
{
    private ISession _session;

    [SetUp]
    public void SetUp_(){ _session = R<ISession>();         }

    [Test]
    public void With_typical_data()
    {
        var user = ContextRegisteredUser.New().Add().Persist().Users[0];
        user.DateCreated = DateTime.Now.AddYears(-2);

        WriteHistory(user, 0);
        WriteHistory(user, -1);
        WriteHistory(user, -10);
        WriteHistory(user, -11);
        WriteHistory(user, -12);
        WriteHistory(user, -13);
        WriteHistory(user, -17);

        var streakResult = R<GetStreaks>().Run(user);
        Assert.That(streakResult.LongestLength, Is.EqualTo(4));
        Assert.That(streakResult.LastLength, Is.EqualTo(2));
    }

    [Test]
    public void With_empty_data()
    {
        var user = ContextRegisteredUser.New().Add().Persist().Users[0];
        user.DateCreated = DateTime.Now.AddYears(-2);

        var streakResult = R<GetStreaks>().Run(user);
        Assert.That(streakResult.LongestLength, Is.EqualTo(0));
        Assert.That(streakResult.LastLength, Is.EqualTo(0));
    }

    private void WriteHistory(User user, int daysOffset)
    {
        _session.Save(
            new AnswerHistory
            {
                UserId = user.Id,
                QuestionId = 1,
                AnswerredCorrectly = AnswerCorrectness.True,
                DateCreated = DateTime.Now.AddDays(daysOffset)
            }
        );
    }
}
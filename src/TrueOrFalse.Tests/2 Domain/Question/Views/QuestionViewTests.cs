using NUnit.Framework;

namespace TrueOrFalse.Tests;

public class QuestionViewTests : BaseTest
{
    [Test]
    public void Should_get_total_views_for_question()
    {
        var questionViewRepository = Resolve<QuestionViewRepository>();
        questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 1 });
        questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 1 });
        questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 2 });
        questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 1 });

        Assert.That(questionViewRepository.GetViewCount(1), Is.EqualTo(3));
        Assert.That(questionViewRepository.GetViewCount(2), Is.EqualTo(1));
        Assert.That(questionViewRepository.GetViewCount(3), Is.EqualTo(0));
    }
}
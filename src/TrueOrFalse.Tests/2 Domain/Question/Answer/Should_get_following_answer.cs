using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_get_following_answer : BaseTest
{
    [Test]
    public void Test()
    {
        var answerRepo = R<AnswerRepo>();
        var contextQuestion = 
            ContextQuestion.New(R<QuestionWritingRepo>(),
                    answerRepo, 
                    R<AnswerQuestion>(),
                    R<UserRepo>(),
                    R<CategoryRepository>(),
                    persistImmediately: true)
                .AddQuestion(questionText: "Some Question", solutionText: "Some answer")
                .AddAnswer("some answer 1")
            .Persist();

        Assert.That(FollowingAnswer.Get(contextQuestion.AllAnswers[0], answerRepo), Is.EqualTo(null));

        contextQuestion
            .AddAnswer("some answer 2")
            .AddAnswer("some answer 3")
            .AddAnswer("some answer 4");

        Assert.That(FollowingAnswer.Get(contextQuestion.AllAnswers[0], answerRepo).AnswerText, Is.EqualTo("some answer 2"));
    }
}
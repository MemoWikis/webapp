using NUnit.Framework;
using TrueOrFalse.Tests;

public class QuestionAnswerPersistenceTests : BaseTest
{
    [Test]
    public void Persistence_Test()
    {
        var answer = new Answer();
        answer.Question = ContextQuestion.GetQuestion(R<QuestionRepo>(), R<AnswerRepo>(), R<AnswerQuestion>());
        answer.MillisecondsSinceQuestionView = 100;
        answer.UserId = 1;
        answer.AnswerText = "asdfasfsf";

        Resolve<AnswerRepo>().Create(answer);
    }
}
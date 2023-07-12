using NUnit.Framework;
using TrueOrFalse.Tests;

public class QuestionAnswerPersistenceTests : BaseTest
{
    [Test]
    public void Persistence_Test()
    {
        var answerRepo = R<AnswerRepo>();
        var answer = new Answer();
        answer.Question = ContextQuestion.GetQuestion(R<QuestionRepo>(), 
            answerRepo, 
            R<AnswerQuestion>(),
            R<UserRepo>());
        answer.MillisecondsSinceQuestionView = 100;
        answer.UserId = 1;
        answer.AnswerText = "asdfasfsf";
        answerRepo.Create(answer);
    }
}
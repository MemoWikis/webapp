using NUnit.Framework;
using TrueOrFalse.Tests;

public class QuestionAnswerPersistenceTests : BaseTest
{
    [Test]
    public void Persistence_Test()
    {
        var answerRepo = R<AnswerRepo>();
        var answer = new Answer();
        answer.Question = ContextQuestion.GetQuestion(answerRepo, 
            R<AnswerQuestion>(),
            R<UserReadingRepo>(),
            R<CategoryRepository>(),
            R<QuestionWritingRepo>());
        answer.MillisecondsSinceQuestionView = 100;
        answer.UserId = 1;
        answer.AnswerText = "asdfasfsf";
        answerRepo.Create(answer);
    }
}
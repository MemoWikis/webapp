using NUnit.Framework;

public class QuestionAnswerHistoryPersistenceTests : BaseTest
{
    [Test]
    public void Persistence_Test()
    {
        var questionHistory = new AnswerHistory();
        questionHistory.QuestionId = 12;
        questionHistory.Milliseconds = 100;
        questionHistory.UserId = 1;
        questionHistory.AnswerText = "asdfasfsf";

        Resolve<AnswerHistoryRepo>().Create(questionHistory);
    }
}
using NUnit.Framework;

public class QuestionAnswerPersistenceTests : BaseTest
{
    [Test]
    public void Persistence_Test()
    {
        var questionHistory = new Answer();
        questionHistory.QuestionId = 12;
        questionHistory.Milliseconds = 100;
        questionHistory.UserId = 1;
        questionHistory.AnswerText = "asdfasfsf";

        Resolve<AnswerRepo>().Create(questionHistory);
    }
}
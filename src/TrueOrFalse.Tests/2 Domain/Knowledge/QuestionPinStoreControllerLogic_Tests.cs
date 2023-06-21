using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using TrueOrFalse.Domain;

namespace TrueOrFalse.Tests._2_Domain.Knowledge;

internal class QuestionPinStoreControllerLogic_Tests : BaseTest
{
    [Test]
    public void PinTestToMuchKnowledge()
    {
        var nameQuestion1 = "Question1";
        var nameQuestion2 = "Question2";
        var nameQuestion3 = "Question3";

        var questionContext = ContextQuestion.New()
            .AddQuestion(nameQuestion1)
            .AddQuestion(nameQuestion2)
            .AddQuestion(nameQuestion3)
            .Persist();

        SessionUser.Login(questionContext.Creator);


        var question1Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion1)).Id;
        var question2Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion2)).Id;
        var question3Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion3)).Id;


        var field = typeof(LimitCheck).GetField("_wishCountKnowledge", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);

        var result1 = new QuestionPinStoreControllerLogic().Pin(question1Id);
        var result2 = new QuestionPinStoreControllerLogic().Pin(question2Id);
        var result3 = new QuestionPinStoreControllerLogic().Pin(question3Id);
        result3 = JsonConvert.SerializeObject(result3);
        var expectedResult = JsonConvert.SerializeObject(new { success = false, key = "cantAddKnowledge" });

        Assert.True(result1);
        Assert.True(result2);
        Assert.AreEqual(expectedResult, result3);
    }

    [Test]
    public void PinTestSubscriptionEndDateValid()
    {
        var nameQuestion1 = "Question1";
        var nameQuestion2 = "Question2";
        var nameQuestion3 = "Question3";

        var questionContext = ContextQuestion.New()
            .AddQuestion(nameQuestion1)
            .AddQuestion(nameQuestion2)
            .AddQuestion(nameQuestion3)
            .Persist();

        questionContext.Creator.EndDate = DateTime.Now.AddDays(1);
        SessionUser.Login(questionContext.Creator);


        var question1Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion1)).Id;
        var question2Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion2)).Id;
        var question3Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion3)).Id;


        var field = typeof(LimitCheck).GetField("_wishCountKnowledge", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);

        var result1 = new QuestionPinStoreControllerLogic().Pin(question1Id);
        var result2 = new QuestionPinStoreControllerLogic().Pin(question2Id);
        var result3 = new QuestionPinStoreControllerLogic().Pin(question3Id);

        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
    }
}
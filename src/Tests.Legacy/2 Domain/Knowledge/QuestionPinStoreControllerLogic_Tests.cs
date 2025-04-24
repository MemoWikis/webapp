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

        var questionContext = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(), 
                R<AnswerQuestion>(),
                R<UserWritingRepo>(), 
                R<CategoryRepository>())
            .AddQuestion(nameQuestion1)
            .AddQuestion(nameQuestion2)
            .AddQuestion(nameQuestion3)
            .Persist();
        var sessionUser = Resolve<SessionUser>(); 
        sessionUser.Login(questionContext.Creator);


        var question1Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion1)).Id;
        var question2Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion2)).Id;
        var question3Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion3)).Id;


        var field = typeof(LimitCheck).GetField("_wishCountKnowledge", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);
        var questionInKnowledge = Resolve<QuestionInKnowledge>(); 
        var result1 = R<QuestionPinStoreControllerLogic>().Pin(question1Id, sessionUser);
            var result2 = R<QuestionPinStoreControllerLogic>().Pin(question2Id, sessionUser);
            var result3 = R<QuestionPinStoreControllerLogic>().Pin(question3Id, sessionUser);
        var expectedResult = new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Subscription.CantAddKnowledge };

        Assert.True(result1.success);
        Assert.True(result2.success);
        Assert.AreEqual(expectedResult, result3);
    }

    [Test]
    public void PinTestSubscriptionEndDateValid()
    {
        var nameQuestion1 = "Question1";
        var nameQuestion2 = "Question2";
        var nameQuestion3 = "Question3";

        var questionContext = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(), 
                R<AnswerQuestion>(),
                R<UserWritingRepo>(), 
                R<CategoryRepository>())
            .AddQuestion(nameQuestion1)
            .AddQuestion(nameQuestion2)
            .AddQuestion(nameQuestion3)
            .Persist();

        questionContext.Creator.EndDate = DateTime.Now.AddDays(1);
        var sessionUser = Resolve<SessionUser>(); 
        sessionUser.Login(questionContext.Creator);


        var question1Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion1)).Id;
        var question2Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion2)).Id;
        var question3Id = questionContext.All
            .First(q => q.Text.Equals(nameQuestion3)).Id;


        var field = typeof(LimitCheck).GetField("_wishCountKnowledge", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);

        var questionInKnowledge = Resolve<QuestionInKnowledge>(); 
        var result1 = R<QuestionPinStoreControllerLogic>().Pin(question1Id, sessionUser);
        var result2 = R<QuestionPinStoreControllerLogic>().Pin(question2Id, sessionUser);
        var result3 = R<QuestionPinStoreControllerLogic>().Pin(question3Id, sessionUser);

        Assert.True(result1.success);
        Assert.True(result2.success);
        Assert.True(result3.success);
    }
}
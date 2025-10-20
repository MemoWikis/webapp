using System.Collections.Generic;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

[Category(TestCategories.Programmer)]
public class QestionValuation_persistence_tests : BaseTest
{
    [Test]
    public void QuestionValuation_should_be_persisted()
    {
        var contextQuestion = ContextQuestion.New(
            R<QuestionWritingRepo>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            R<UserWritingRepo>(),
            R<CategoryRepository>())
            .AddQuestion(questionText: "a", solutionText: "b").Persist();
        var questionValuation = 
            new QuestionValuation  
            {
                Question = contextQuestion.All[0],
                User = contextQuestion.Creator,
                Quality = 91,
                RelevanceForAll = 40,
                RelevancePersonal = 7
            };

        Resolve<QuestionValuationRepo>().Create(questionValuation);
    }

    [Test]
    public void Should_select_by_question_ids()
    {
        var userWritingRepo = R<UserWritingRepo>(); 
        var context = ContextQuestion.New(
                R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                userWritingRepo,
                R<CategoryRepository>())
            .AddQuestion(questionText: "1", solutionText: "a")
            .AddQuestion(questionText: "2", solutionText: "a")
            .AddQuestion(questionText: "3", solutionText: "a")
            .AddQuestion(questionText: "4", solutionText: "a")
            .AddQuestion(questionText: "5", solutionText: "a")
            .Persist();

        var contextUsers = ContextUser.New(userWritingRepo)
            .Add("User1")
            .Add("User2")
            .Persist();

        var user1 = contextUsers.All[0];
        var user2 = contextUsers.All[1];

        var questionValuation1 = new QuestionValuation { Question = context.All[0], User = user1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
        var questionValuation2 = new QuestionValuation { Question = context.All[1], User = user1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
        var questionValuation3 = new QuestionValuation { Question = context.All[2], User = user2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
        var questionValuation4 = new QuestionValuation { Question = context.All[3], User = user1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
        var questionValuation5 = new QuestionValuation { Question = context.All[4], User = user2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };

        Resolve<QuestionValuationRepo>().Create(
            new List<QuestionValuation> { questionValuation1, questionValuation2, questionValuation3, questionValuation4, questionValuation5});

        Assert.That(Resolve<QuestionValuationRepo>().GetActiveInWishKnowledge(
            new[] { context.All[0].Id, context.All[1].Id, context.All[2].Id }, user1.Id).Count, Is.EqualTo(2));
    }
}
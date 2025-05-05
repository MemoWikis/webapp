using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using Seedworks.Web.State;

namespace TrueOrFalse.Tests;

public class UserValuationCache_tests : BaseTest
{
    [Test]
    [Ignore("failing")]
    public void Run()
    {
        ContextCategory.New().Add("1").Add("2").Add("3").Persist();

        var categoryRepo = R<CategoryRepository>();
        var category1 = categoryRepo.GetByName("1").FirstOrDefault();
        var category2 = categoryRepo.GetByName("2").FirstOrDefault();
        var category3 = categoryRepo.GetByName("3").FirstOrDefault();

        var userWritingRepo = R<UserWritingRepo>();
        ContextQuestion.New(R<QuestionWritingRepo>() ,R<AnswerRepo>(), R<AnswerQuestion>(), userWritingRepo, categoryRepo)
            .AddQuestion(questionText: "Question1", solutionText: "Answer", categories: new List<Category> { category1 })
            .AddQuestion(questionText: "Question2", solutionText: "Answer", categories: new List<Category> { category2 })
            .AddQuestion(questionText: "Question3", solutionText: "Answer", categories: new List<Category> { category3 })
            .Persist();

        var user = ContextUser.GetUser(userWritingRepo);

        RecycleContainer();

        Assert.That(HttpRuntime.Cache.Count, Is.EqualTo(0));
        Assert.That(Cache.Count, Is.EqualTo(0));

        var userReadingRepo = R<UserReadingRepo>();
        var questionValuationRepo = R<QuestionValuationRepo>();
        var categoryValuationReadingRepo = R<CategoryValuationReadingRepo>();
        var cacheItem = SessionUserCache.GetItem(user.Id, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

        Assert.That(cacheItem.CategoryValuations.Count, Is.EqualTo(3));

        cacheItem.CategoryValuations.TryRemove(cacheItem.CategoryValuations.Keys.First(), out var catValout);

        var cacheItem2 = SessionUserCache.GetItem(user.Id, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

        Assert.That(cacheItem2.CategoryValuations.Count, Is.EqualTo(2));
        Assert.That(HttpRuntime.Cache.Count, Is.EqualTo(1));
    }

    [Test]
    [Ignore("failing")]
    public void Foo()
    {
        var knowledgeSummary = Resolve<KnowledgeSummaryLoader>().RunFromDbCache(-1, -1);
    }
}
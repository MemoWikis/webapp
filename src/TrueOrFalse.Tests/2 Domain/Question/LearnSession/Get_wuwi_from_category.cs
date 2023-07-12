using System.Linq;
using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

class Get_wuwi_from_category : BaseTest
{
    [Test]
    public void GetWuwiSession()
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(R<CategoryRepository>(), R<QuestionRepo>(), R<AnswerRepo>(),R<AnswerQuestion>());
        ContextQuestion.SetWuwi(20, R<CategoryValuationRepo>(), R<QuestionRepo>(), R<AnswerRepo>(), R<AnswerQuestion>(), R<UserRepo>(), R<QuestionValuationRepo>());
            
        var categoryId = 1;
        var userCacheItem = SessionUserCache.GetAllCacheItems(
            Resolve<CategoryValuationRepo>(), R<UserRepo>(), R<QuestionValuationRepo>())
            .First(uci => uci.Name == "Daniel" );

        var wuwis = userCacheItem.QuestionValuations
            .Select(qv => qv.Value)
            .Where(qv=> qv.IsInWishKnowledge && qv.Question.Categories.Any(c=> c.Id == categoryId) )
            .ToList();

        var wuwisFromLearningSession = Resolve<LearningSessionCreator>().BuildLearningSession(new LearningSessionConfig
            { InWuwi = true, CategoryId = categoryId, CurrentUserId = userCacheItem.Id});

        Assert.That(wuwisFromLearningSession.Steps.Count, Is.EqualTo(wuwis.Count));
    }
}
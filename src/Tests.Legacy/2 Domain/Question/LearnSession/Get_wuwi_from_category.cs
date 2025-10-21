using System.Linq;
using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

class Get_wuwi_from_category : BaseTest
{
    [Test]
    public void GetWuwiSession()
    {
        var questionwriterRepo = R<QuestionWritingRepo>(); 
        var userReadingRepo = R<UserReadingRepo>();
        var answerQuestion = R<AnswerQuestion>();
        var answerRepo = R<AnswerRepo>();
        var userWritingRepo = R<UserWritingRepo>();
        ContextQuestion.PutQuestionsIntoMemoryCache(R<CategoryRepository>(),
            answerRepo,
            answerQuestion,
            userWritingRepo,
            questionwriterRepo);

        var questionValuationRepo = R<QuestionValuationRepo>();
        ContextQuestion.SetWuwi(20,
            R<CategoryValuationReadingRepo>(),
            answerRepo,
            answerQuestion,
            userReadingRepo,
            questionValuationRepo,
            R<CategoryRepository>(),
            questionwriterRepo,
            userWritingRepo);
            
        var categoryId = 1;
        var userCacheItem = SessionUserCache.GetAllCacheItems(
            Resolve<CategoryValuationReadingRepo>(), userReadingRepo, questionValuationRepo)
            .First(uci => uci.Name == "Daniel" );

        var wuwis = userCacheItem.QuestionValuations
            .Select(qv => qv.Value)
            .Where(qv=> qv.IsInWishKnowledge && qv.Question.Categories.Any(c=> c.Id == categoryId) )
            .ToList();

        var wuwisFromLearningSession = Resolve<LearningSessionCreator>().BuildLearningSession(new LearningSessionConfig
            { InWishKnowledge = true, CategoryId = categoryId, CurrentUserId = userCacheItem.Id});

        Assert.That(wuwisFromLearningSession.Steps.Count, Is.EqualTo(wuwis.Count));
    }
}
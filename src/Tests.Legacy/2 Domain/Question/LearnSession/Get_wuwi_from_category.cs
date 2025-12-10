using System.Linq;
using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

class Get_wishKnowledge_from_category : BaseTest
{
    [Test]
    public void GetWishKnowledgeSession()
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
        ContextQuestion.SetWishKnowledge(20,
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

        var wishKnowledges = userCacheItem.QuestionValuations
            .Select(qv => qv.Value)
            .Where(qv=> qv.IsInWishKnowledge && qv.Question.Categories.Any(c=> c.Id == categoryId) )
            .ToList();

        var wishKnowledgesFromLearningSession = Resolve<LearningSessionCreator>().BuildLearningSession(new LearningSessionConfig
            { InWishKnowledge = true, CategoryId = categoryId, CurrentUserId = userCacheItem.Id});

        Assert.That(wishKnowledgesFromLearningSession.Steps.Count, Is.EqualTo(wishKnowledges.Count));
    }
}
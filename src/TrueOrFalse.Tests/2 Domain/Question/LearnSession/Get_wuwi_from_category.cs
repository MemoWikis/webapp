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
        var userRepo = R<UserRepo>();
        var answerQuestion = R<AnswerQuestion>();
        var answerRepo = R<AnswerRepo>();
        ContextQuestion.PutQuestionsIntoMemoryCache(R<CategoryRepository>(),
            answerRepo,
            answerQuestion,
            userRepo,
            questionwriterRepo);

        var questionValuationRepo = R<QuestionValuationRepo>();
        ContextQuestion.SetWuwi(20,
            R<CategoryValuationReadingRepo>(),
            answerRepo,
            answerQuestion,
            userRepo,
            questionValuationRepo,
            R<CategoryRepository>(),
            questionwriterRepo);
            
        var categoryId = 1;
        var userCacheItem = SessionUserCache.GetAllCacheItems(
            Resolve<CategoryValuationReadingRepo>(), userRepo, questionValuationRepo)
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
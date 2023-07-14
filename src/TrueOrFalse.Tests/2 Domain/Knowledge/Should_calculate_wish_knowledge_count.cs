using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

public class Should_calculate_wish_knowledge_count : BaseTest
{
    [Test]
    public void Run()
    {
        var contextQuestion = ContextQuestion.New(R<QuestionRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserRepo>(), 
                R<CategoryRepository>(), 
                R<QuestionWritingRepo>())
            .AddQuestion(questionText: "QuestionA", solutionText: "AnswerA").AddCategory("A", R<EntityCacheInitializer>())
            .Persist();

        var question = contextQuestion.All.First();
        var user = contextQuestion.Creator;
        var questKnow = R<QuestionInKnowledge>();
        questKnow.Create(new QuestionValuation { RelevancePersonal = 100, Question = question, User = user });
        questKnow.Create(new QuestionValuation { RelevancePersonal = 1, Question = question, User = user });
        questKnow.Create(new QuestionValuation { Question = question, User = user });

        Assert.That(Resolve<GetWishQuestionCount>().Run(userId: 2), Is.EqualTo(2));
    }
}
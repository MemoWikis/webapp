using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

public class Question_persist_and_load_answer_type : BaseTest
{
    [Test]
    public void Should_store_answer_type()
    {
        var entityCacheInitilizer = LifetimeScope.Resolve<EntityCacheInitializer>();
        var context = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserReadingRepo>(),
                R<CategoryRepository>())
            .AddQuestion(questionText: "What is BDD", solutionText: "Another name for writing acceptance tests")
            .AddCategory("A", entityCacheInitilizer)
            .AddCategory("B", entityCacheInitilizer)
            .AddCategory("C", entityCacheInitilizer)
            .AddQuestion(questionText: "Another Question", solutionText: "Some answer")
            .Persist();

        var question = context.All[0];
    }
}
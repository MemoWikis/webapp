using NUnit.Framework;

namespace TrueOrFalse.Tests;

public class Spec_changing_question_categories : BaseTest
{
    [Test]
    public void Should_change_categories()
    {
        var entityCacheInitilizer = R<EntityCacheInitializer>(); 
        var questionContext =
            ContextQuestion.New(R<QuestionWritingRepo>(),
                    R<AnswerRepo>(),
                    R<AnswerQuestion>(),
                    R<UserWritingRepo>(), 
                    R<CategoryRepository>())
                .AddQuestion(questionText: "Question", solutionText: "Answer")
                .AddCategory("A", entityCacheInitilizer)
                .AddCategory("B", entityCacheInitilizer)
                .AddCategory("C", entityCacheInitilizer)
                .Persist();

        var question = questionContext.All[0];
        var categoryA = question.Categories.ByName("A");
        var categoryB = question.Categories.ByName("B");

        question.Categories.Clear();
        question.Categories.Add(categoryA);
        question.Categories.Add(categoryB);

    
        Resolve<QuestionWritingRepo>().UpdateOrMerge(question, false);

        var questionFromDb = Resolve<QuestionWritingRepo>().GetAll()[0];
        Assert.That(questionFromDb.Categories.Count, Is.EqualTo(2));
        Assert.That(questionFromDb.Categories[0].Name, Is.EqualTo("A"));
        Assert.That(questionFromDb.Categories[1].Name, Is.EqualTo("B"));
    }
}
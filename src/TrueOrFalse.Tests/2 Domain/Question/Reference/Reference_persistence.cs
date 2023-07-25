using System.Linq;
using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence;

[Category(TestCategories.Programmer)]
public class Reference_persistence : BaseTest
{
    [Test]
    public void Should_persist_reference()
    {
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
            R<AnswerRepo>(), 
            R<AnswerQuestion>(), 
            R<UserRepo>(), 
            R<CategoryRepository>()).AddQuestion(questionText: "text", solutionText: "solution").Persist();
        var contextCategory = ContextCategory.New().Add("categoryName").Persist();
            
        var reference = new Reference();
        reference.Question = contextQuestion.All.First();
        reference.Category = contextCategory.All.First();
        reference.AdditionalInfo = "Additional Info";
        reference.ReferenceText = "Free text reference";

        R<ReferenceRepo>().Create(reference);

        var references = R<ReferenceRepo>().GetAll();
        Assert.That(references[0].Category.Name, Is.EqualTo(contextCategory.All.First().Name));
    }

    [Test]
    public void Should_persist_reference_without_category()
    {
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(), 
                R<AnswerQuestion>(), 
                R<UserRepo>(), 
                R<CategoryRepository>())
            .AddQuestion(questionText: "text", solutionText: "solution").Persist();

        var reference = new Reference();
        reference.Question = contextQuestion.All.First();
        reference.AdditionalInfo = "Additional Info";
        reference.ReferenceText = "Free text reference";

        R<ReferenceRepo>().Create(reference);

        var references = R<ReferenceRepo>().GetAll();            
        Assert.That(references.Count, Is.EqualTo(1));
        Assert.That(references[0].Category, Is.EqualTo(null));

        var allReferences = R<ISession>()
            .QueryOver<Reference>()
            .Where(r => r.Question.Id == contextQuestion.All[0].Id)
            .List();

        Assert.That(allReferences.Count, Is.EqualTo(1));
    }

    [Test]
    public void Should_map_references_to_question()
    {
        //Arange
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
            R<AnswerRepo>(), 
            R<AnswerQuestion>(), 
            R<UserRepo>(), 
            R<CategoryRepository>())
            .AddQuestion()
            .Persist();
        var question = contextQuestion.All[0];
        question.References.Add(new Reference{ReferenceText = "FTR"});
        question.References.Add(new Reference{AdditionalInfo = "AI"});

        //Act
        R<QuestionWritingRepo>().UpdateOrMerge(question, false);

        //Assert
        RecycleContainer();
        var questionFromDb = R<QuestionReadingRepo>().GetAll()[0];
        Assert.That(questionFromDb.References.Count, Is.EqualTo(2));
        Assert.That(questionFromDb.References[0].Question, Is.EqualTo(questionFromDb));
    }

        
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDDish.Model;
using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Reference_persistence : BaseTest
    {
        [Test]
        public void Should_persist_reference()
        {
            var contextQuestion = ContextQuestion.New().AddQuestion("text", "solution").Persist();
            var contextCategory = ContextCategory.New().Add("categoryName").Persist();
            
            var reference = new Reference();
            reference.Question = contextQuestion.All.First();
            reference.Category = contextCategory.All.First();
            reference.AdditionalInfo = "Additional Info";
            reference.FreeTextReference = "Free text reference";

            R<ReferenceRepository>().Create(reference);

            var references = R<ReferenceRepository>().GetAll();
            Assert.That(references[0].Category.Name, Is.EqualTo(contextCategory.All.First().Name));
        }

        [Test]
        public void Should_persist_reference_without_category()
        {
            var contextQuestion = ContextQuestion.New().AddQuestion("text", "solution").Persist();

            var reference = new Reference();
            reference.Question = contextQuestion.All.First();
            reference.AdditionalInfo = "Additional Info";
            reference.FreeTextReference = "Free text reference";

            R<ReferenceRepository>().Create(reference);

            var references = R<ReferenceRepository>().GetAll();            
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
            var contextQuestion = ContextQuestion.New().AddQuestion().Persist();
            var question = contextQuestion.All[0];
            question.References.Add(new Reference{FreeTextReference = "FTR"});
            question.References.Add(new Reference{AdditionalInfo = "AI"});

            //Act
            R<QuestionRepository>().Update(question);

            //Assert
            RecycleContainer();

            var questionFromDb = R<QuestionRepository>().GetAll()[0];
            Assert.That(questionFromDb.References.Count, Is.EqualTo(2));
            Assert.That(questionFromDb.References[0].Question, Is.EqualTo(questionFromDb));


            questionFromDb.References = new Reference[]{};
            R<QuestionRepository>().Update(questionFromDb);
        }

        [Test]
        public void Should_delete_reference_with_question()
        {
            var reference1 = Reference();
            var reference2 = Reference();

            R<ReferenceRepository>().Create(reference1);
            R<ReferenceRepository>().Create(reference2);
            RecycleContainer();

            var session = R<ISession>();
            var questionFromDb1 = session.QueryOver<Question>().List()[0];
            var questionFromDb2 = session.QueryOver<Question>().List()[1];
            
            session.Delete(questionFromDb1);
            questionFromDb2.References.Remove(reference2);

            RecycleContainer();
        }

        private static Reference Reference()
        {
            var contextQuestion = ContextQuestion.New().AddQuestion("text", "solution").Persist();
            var contextCategory = ContextCategory.New().Add("categoryName").Persist();

            var reference = new Reference();
            reference.Question = contextQuestion.All.First();
            reference.Category = contextCategory.All.First();
            reference.AdditionalInfo = "Additional Info";
            reference.FreeTextReference = "Free text reference";
            return reference;
        }
    }
}

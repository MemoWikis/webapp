﻿using System;
using System.Linq;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Question_persistence : BaseTest
    {
        [Test]
        public void Questions_should_be_persisted()
        {
            var context = ContextQuestion.New().AddQuestion("What is BDD", "Another name for writing acceptance tests")
                                    .AddCategory("A")
                                    .AddCategory("B")
                                    .AddCategory("C")
                                 .AddQuestion("Another Question", "Some answer")
                                    .Persist();
            
            Resolve<ISession>().Evict(context.All[0]);

            var questions = Resolve<QuestionRepository>().GetAll();
            questions.Count.Should().Be.EqualTo(2);
            questions[0].Categories.Count.Should().Be.EqualTo(3);
            questions[0].Categories[0].Name.Should().Be.EqualTo("A");
            questions[0].Categories[2].Name.Should().Be.EqualTo("C");
            questions[0].Solution.StartsWith("Another").Should().Be.True();
            questions[1].Solution.StartsWith("Some").Should().Be.True();
        }

        [Test]
        public void Should_ensure_correct_cascading_for_categories()
        {
            //Arrange
            var context = ContextQuestion.New()
                    .AddQuestion("Q")
                        .AddCategory("C")
                    .Persist();
            
            RecycleContainer();

            //Act
            var session = R<ISession>();
            var questionFromDb = session.QueryOver<Question>().List()[0];
            session.Delete(questionFromDb);

            RecycleContainer();
            session = R<ISession>();
            
            //Assert
            Assert.That(session.QueryOver<Question>().List().Count, Is.EqualTo(0));
            Assert.That(session.QueryOver<Category>().List().Count, Is.EqualTo(1));
            //Assert SaveUpdate?
        }

        [Test]
        public void Should_ensure_correct_cascading_for_references()
        {
            var reference1 = ReferenceWithContext();
            var reference2 = ReferenceWithContext();

            R<ReferenceRepository>().Create(reference1);
            R<ReferenceRepository>().Create(reference2);
            RecycleContainer();

            var session = R<ISession>();
            var questionFromDb1 = session.QueryOver<Question>().List()[0];
            var questionFromDb2 = session.QueryOver<Question>().List()[1];

            session.Delete(questionFromDb1);//Should delete contained references when question is deleted
            questionFromDb2.References.Remove(reference2);//Should delete contained reference when reference is removed from collection

            RecycleContainer();
            session = R<ISession>();

            Assert.That(session.QueryOver<Question>().List().Count, Is.EqualTo(1));
            Assert.That(session.QueryOver<Category>().List().Count, Is.EqualTo(2));
            Assert.That(session.QueryOver<Reference>().List().Count, Is.EqualTo(0));
        }

        private static Reference ReferenceWithContext()
        {
            var uniqueCategoryName = "Category" + Guid.NewGuid();
            var contextQuestion = ContextQuestion.New().AddQuestion("text", "solution").Persist();
            var contextCategory = ContextCategory.New().Add(uniqueCategoryName).Persist();

            var reference = new Reference();
            reference.Question = contextQuestion.All.First();
            reference.Category = contextCategory.All.First();
            reference.AdditionalInfo = "Additional Info";
            reference.ReferenceText = "Reference Text";
            return reference;
        }
    }
}

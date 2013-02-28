using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class QuestionSet_add_question : BaseTest
    {
        [Test]
        public void Should_add_question_to_questionSet()
        {        
            var context = ContextQuestion.New()
                .AddQuestion("Q1", "A1").AddCategory("A")
                .AddQuestion("Q2", "A2").AddCategory("A")
                .Persist();

            var questionSetRepo = Resolve<QuestionSetRepository>();
            var questionSet = new QuestionSet {Name = "QS1"};
            questionSetRepo.Create(questionSet);

            Resolve<AddToQuestionSet>().Run(context.Questions.GetIds().ToArray(), questionSet.Id);

            base.RecycleContainer();
            var questionSetFromDb = Resolve<QuestionSetRepository>().GetById(questionSet.Id);
            var questionSetFromDb2 = Resolve<ISession>().QueryOver<QuestionSet>().SingleOrDefault();

            Assert.That(questionSetFromDb2.QuestionsInSet.Count, Is.EqualTo(2));
            Assert.That(questionSetFromDb.QuestionsInSet.Count, Is.EqualTo(2));
            Assert.That(Resolve<QuestionInSetRepo>().GetAll().Count(), Is.EqualTo(2));
            Assert.That(Resolve<QuestionInSetRepo>().GetAll().First().QuestionSet.Id, Is.EqualTo(1));
        }
    }
}

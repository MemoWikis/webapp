using System.Linq;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class QuestionValuation_add_totals_test : BaseTest
    {
        [Test]
        public void Should_update_question_totals()
        {
            var contextQuestion = ContextQuestion.New()
                .AddQuestion("QuestionA", "AnswerA").AddCategory("A")
                .Persist();

            var questionVal1 = new QuestionValuation
                                    {
                                        RelevanceForAll = 50,
                                        RelevancePersonal = 100,
                                        QuestionId = contextQuestion.Questions.First().Id,
                                        UserId = 2
                                    };

            var questionVal2 = new QuestionValuation
                                   {
                                       RelevanceForAll = 10,
                                       RelevancePersonal = 80,
                                       QuestionId = contextQuestion.Questions.First().Id,
                                       UserId = 3
                                   };

            var questionTotals = Resolve<UpdateQuestionTotals>();
            questionTotals.Run(questionVal1);
            questionTotals.Run(questionVal2);

            Resolve<ISession>().Evict(contextQuestion.Questions.First());

            var question = Resolve<QuestionRepository>().GetById(contextQuestion.Questions.First().Id);
            Assert.That(question.TotalQualityEntries, Is.EqualTo(0));
            Assert.That(question.TotalQualityAvg, Is.EqualTo(0));
            Assert.That(question.TotalRelevanceForAllAvg, Is.EqualTo(50));
            Assert.That(question.TotalRelevanceForAllEntries, Is.EqualTo(1));
            Assert.That(question.TotalRelevancePersonalAvg, Is.EqualTo(100));
            Assert.That(question.TotalRelevancePersonalEntries, Is.EqualTo(1));
        }
    }
}

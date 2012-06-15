using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class QuestionValuation_add_totals_test : BaseTest
    {
        public void Should_update_question_totals()
        {
            var contextQuestion = ContextQuestion.New()
                .AddQuestion("QuestionA", "AnswerA").AddCategory("A")
                .Persist();

            var questionVal1 = new QuestionValuation
                                    {
                                        RelevanceForAll = 50,
                                        RelevancePesonal = 100,
                                        QuestionId = contextQuestion.Questions.First().Id,
                                        UserId = 2
                                    };

            var questionVal2 = new QuestionValuation
                                   {
                                       RelevanceForAll = 10,
                                       RelevancePesonal = 80,
                                       QuestionId = contextQuestion.Questions.First().Id,
                                       UserId = 3
                                   };

            var questionTotals = Resolve<UpdateQuestionTotals>();
            questionTotals.Run(questionVal1);

            //Resolve<QuestionValuationRepository>()
            //    .Create(new List<QuestionValuation> { questionVal1, questionVal2 });
        }

    }
}

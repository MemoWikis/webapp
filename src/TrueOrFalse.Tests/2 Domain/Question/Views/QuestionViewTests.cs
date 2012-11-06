using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class QuestionViewTests : BaseTest
    {
        [Test]
        public void Should_get_total_views_for_question()
        {
            var questionViewRepository = Resolve<QuestionViewRepository>();
            questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 1 });
            questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 1 });
            questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 2 });
            questionViewRepository.Create(new QuestionView { UserId = 1, QuestionId = 1 });

            Assert.That(questionViewRepository.GetViewCount(1), Is.EqualTo(3));
            Assert.That(questionViewRepository.GetViewCount(2), Is.EqualTo(1));
            Assert.That(questionViewRepository.GetViewCount(3), Is.EqualTo(0));
        }

        [Test]
        public void Should_store_total_view_count_in_question_after_every_view()
        {
            var contextQuestion = ContextQuestion.New()
                    .AddQuestion("QuestionA", "AnswerA").AddCategory("A")
                    .Persist();

            var questionId = contextQuestion.Questions[0].Id;
            Resolve<SaveQuestionView>().Run(questionId, 2);
            Resolve<SaveQuestionView>().Run(questionId, 2);
            Resolve<ISession>().Evict(contextQuestion.Questions[0]);

            Assert.That(Resolve<QuestionRepository>().GetById(questionId).TotalViews, Is.EqualTo(2));
        }
    }
}

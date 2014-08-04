using NHibernate.Event;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class When_answering_question : BaseTest
    {
        [Test]
        public void It_should_be_create_answer_history_item_and_knowledge_item()
        {
            var contextUsers = ContextRegisteredUser.New().Add().Persist();
            var contextQuestion = ContextQuestion.New()
                    .AddQuestion("Some Question", "Some answer")
                    .AddCategory("A").
                Persist();

            var createdQuestion = contextQuestion.All[0];
            var user = contextUsers.Users[0];
            Resolve<AnswerQuestion>().Run(createdQuestion.Id, "Some answer", user.Id);

            var answerHistoryItems = Resolve<AnswerHistoryRepository>().GetAll();
            Assert.That(answerHistoryItems.Count, Is.EqualTo(1));
            Assert.That(answerHistoryItems[0].AnswerText, Is.EqualTo("Some answer"));
            Assert.That(answerHistoryItems[0].UserId, Is.EqualTo(user.Id));

            var questionValuationRepo = Resolve<QuestionValuationRepository>();
            Assert.That(questionValuationRepo.GetBy(createdQuestion.Id, user.Id).CorrectnessProbability, Is.EqualTo(100));
        }
    }
}

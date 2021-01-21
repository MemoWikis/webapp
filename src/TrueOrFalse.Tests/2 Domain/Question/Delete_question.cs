using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Delete_question : BaseTest
    {
        [Test]
        public void Delete_question_right_after_creation()
        {
            var user1 = ContextUser.New().Add("User1").Persist().All.First();
            Sl.SessionUser.Login(user1);

            var contextQuestion = ContextQuestion.New().AddQuestion(creator: user1).Persist();
            var question1 = contextQuestion.All[0];
            QuestionDelete.Run(question1.Id);
            Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(0));
        }

        [Test]
        public void Dont_delete_question_while_it_is_in_other_users_wishknowledge()
        {
            var contextUser = ContextUser.New().Add("User1").Add("User2").Persist();
            var user1 = contextUser.All[0];
            var user2 = contextUser.All[1];
            
            var contextQuestion = ContextQuestion.New()
                .PersistImmediately()
                .AddQuestion(creator: user1)
                .AddToWishknowledge(user2);
            var question1 = contextQuestion.All[0];

            RecycleContainer();
            user2 = R<UserRepo>().GetById(user2.Id);
            question1 = R<QuestionRepo>().GetById(question1.Id);
            Sl.SessionUser.Login(user1);

            Assert.That(user2.WishCountQuestions, Is.EqualTo(1));
            Assert.That(question1.TotalRelevancePersonalEntries, Is.EqualTo(1));

            var ex = Assert.Throws<Exception>(() => QuestionDelete.Run(question1.Id));
            Assert.That(ex.Message, Contains.Substring("Question cannot be deleted"));

            //now user2 removes the question from his WishKnowledge, so user1 can delete his question
            QuestionInKnowledge.Unpin(question1.Id, user2);
            QuestionDelete.Run(question1.Id);
            Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(0));
        }

        [Test]
        public void Delete_question_even_if_it_has_been_trained()
        {
            //Scenario: User1 creates question1. User1 and User2 answer/learn that question. User1 can still delete the question.
            var contextUser = ContextUser.New().Add("User1").Add("User2").Persist();
            var user1 = contextUser.All[0];
            var user2 = contextUser.All[1];
            Sl.SessionUser.Login(user1);
            var contextQuestion = ContextQuestion.New()
                .PersistImmediately()
                .AddQuestion(creator: user1);
            var question1 = contextQuestion.All[0];

            Resolve<AnswerQuestion>().Run(question1.Id, "Some answer", user1.Id, Guid.NewGuid(), 1, -1);
            Resolve<AnswerQuestion>().Run(question1.Id, "Some answer", user2.Id, Guid.NewGuid(), 1, -1);

            QuestionDelete.Run(question1.Id);
            Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(0));
            var answers = Resolve<AnswerRepo>().GetAll();
            Assert.That(answers.Count, Is.EqualTo(0));
        }

        [Test]
        [Ignore("")]
        public void Dont_delete_question_while_it_is_in_future_date()
        {
            //Scenario: User1 creates 10 questions for a set that user2 uses for a date. As long as date is in future, questions included in this date cannot be deleted
            var contextUser = ContextUser.New().Add("User1").Add("User2").Persist();
            var user1 = contextUser.All[0];
            var user2 = contextUser.All[1];
            
            var contextQuestions = ContextQuestion.New()
                .PersistImmediately()
                .AddQuestions(10, user1);

            var question1 = contextQuestions.All[0];

            RecycleContainer();
            question1 = R<QuestionRepo>().GetById(question1.Id);

            var ex = Assert.Throws<Exception>(() => QuestionDelete.Run(question1.Id));
            Assert.That(ex.Message.Length > 0);

            QuestionDelete.Run(question1.Id);
            Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(9));
        }
    }
}

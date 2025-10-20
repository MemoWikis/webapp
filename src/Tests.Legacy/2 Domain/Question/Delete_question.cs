using System;
using System.Linq;
using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

class Delete_question : BaseTest
{
    [Test]
    public void Delete_question_right_after_creation()
    {
        var user1 = ContextUser.New(R<UserWritingRepo>()).Add("User1").Persist().All.First();
        Resolve<SessionUser>().Login(user1);

        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(), 
            R<AnswerRepo>(), 
            R<AnswerQuestion>(), 
            R<UserWritingRepo>(), 
            R<CategoryRepository>())
            .AddQuestion(creator: user1)
            .Persist();

        var question1 = contextQuestion.All[0];
        Resolve<QuestionDelete>().Run(question1.Id);
        Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(0));
    }

    [Test]
    public void Dont_delete_question_while_it_is_in_other_users_wishKnowledge()
    {
        var userReadingRepo = R<UserReadingRepo>();
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo).Add("User1").Add("User2").Persist();
        var user1 = contextUser.All[0];
        var user2 = contextUser.All[1];
            
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(), 
                R<AnswerQuestion>(),
                userWritingRepo,
                R<CategoryRepository>())
            .PersistImmediately()
            .AddQuestion(creator: user1)
            .AddToWishKnowledge(user2, LifetimeScope.Resolve<QuestionInKnowledge>());
        var question1 = contextQuestion.All[0];

        RecycleContainer();
        user2 = userReadingRepo.GetById(user2.Id);
        question1 = R<QuestionReadingRepo>().GetById(question1.Id);
        Resolve<SessionUser>().Login(user1);

        Assert.That(user2.WishCountQuestions, Is.EqualTo(1));
        Assert.That(question1.TotalRelevancePersonalEntries, Is.EqualTo(1));

        var ex = Assert.Throws<Exception>(() => Resolve<QuestionDelete>().Run(question1.Id));
        Assert.That(ex.Message, Contains.Substring("Question cannot be deleted"));

        //now user2 removes the question from his WishKnowledge, so user1 can delete his question
        Resolve<QuestionInKnowledge>().Unpin(question1.Id, user2.Id);
        Resolve<QuestionDelete>().Run(question1.Id);
        Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(0));
    }

    [Test]
    public void Delete_question_even_if_it_has_been_trained()
    {
        //Scenario: User1 creates question1. User1 and User2 answer/learn that question. User1 can still delete the question.
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo)
            .Add("User1")
            .Add("User2")
            .Persist();

        var user1 = contextUser.All[0];
        var user2 = contextUser.All[1];
        Resolve<SessionUser>().Login(user1);
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(), 
                R<AnswerQuestion>(),
                userWritingRepo, 
                R<CategoryRepository>())
            .PersistImmediately()
            .AddQuestion(creator: user1);
        var question1 = contextQuestion.All[0];

        Resolve<AnswerQuestion>().Run(question1.Id, "Some answer", user1.Id, Guid.NewGuid(), 1, -1);
        Resolve<AnswerQuestion>().Run(question1.Id, "Some answer", user2.Id, Guid.NewGuid(), 1, -1);

        Resolve<QuestionDelete>().Run(question1.Id);
        Assert.That(Resolve<QuestionGetCount>().Run(user1.Id), Is.EqualTo(0));
        var answers = Resolve<AnswerRepo>().GetAll();
        Assert.That(answers.Count, Is.EqualTo(0));
    }
}
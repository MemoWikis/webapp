using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDDish.Model;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse.Tests;


namespace TrueOrFalse.Tests.Persistence
{
    public class Comment_persistence : BaseTest
    {
        [Test]
        public void Comments_should_be_persisted()
        {
            var context = ContextComment.New();
            context.Add("A").Add("B").Persist();
            context.Add("C", commentTo:context.All[0]).Persist();

            var allComments = Resolve<CommentRepository>().GetAll();
            Assert.That(allComments.Count, Is.EqualTo(3));

            Resolve<ISession>().Clear();

            var comments = Resolve<CommentRepository>().GetForDisplay(context.Question.Id);
            Assert.That(comments[0].Answers[0].Text, Is.EqualTo("C"));
        }
    }    
}
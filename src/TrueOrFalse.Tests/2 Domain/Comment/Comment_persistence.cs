using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDDish.Model;
using NUnit.Framework;
using TrueOrFalse.Tests;


namespace TrueOrFalse.Tests.Persistence
{
    public class Comment_persistence : BaseTest
    {
        [Test]
        public void Comments_should_be_persisted()
        {
            ContextComment.New().Add().Add().Add().Persist();

            var allComments = Resolve<CommentRepository>().GetAll();
            Assert.That(allComments.Count, Is.EqualTo(3));
        }
    }    
}




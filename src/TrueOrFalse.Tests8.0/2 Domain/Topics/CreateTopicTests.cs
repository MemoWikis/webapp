using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TrueOrFalse.Tests8._0.Domain.Topics
{
    internal class CreateTopicTests : BaseTest
    {
        [Test]
        public void CreateTopicInDatabase_Test()
        {
            var context = ContextCategory.New();
            var parentname = "Parent";
            var parent = context.Add(parentname).Persist().All.Single(c => c.Name.Equals(parentname));
            var sessionUser = R<SessionUser>();


            var childName = "child";
            var child = R<CategoryCreator>().Create(childName, parent.Id, sessionUser);

            var childFromDatabase = R<CategoryRepository>().GetByName(childName).Single();
            DateTime referenceDate = DateTime.Now;


            Assert.IsNotNull(childFromDatabase);
            Assert.IsNotNull(sessionUser);
            Assert.IsNotNull(childFromDatabase.Creator);
            Assert.IsNotNull(childFromDatabase.DateCreated);
            Assert.IsNotNull(childFromDatabase.DateModified);
            Assert.AreEqual(childName, childFromDatabase.Name);
            Assert.AreEqual(sessionUser.User.Id, childFromDatabase.Creator.Id);
            Assert.AreEqual(sessionUser.User.Name, childFromDatabase.Creator.Name);
            Assert.That(childFromDatabase.DateCreated, Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
            Assert.That(childFromDatabase.DateModified, Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1))); 
            Assert.AreEqual(childFromDatabase.CategoryRelations.Count, 1);
            Assert.AreEqual(childFromDatabase.CategoryRelations.First().RelatedCategory.Name, parent.Name);
        }
    }
}
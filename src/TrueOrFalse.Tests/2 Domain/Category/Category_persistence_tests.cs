using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Category_persistence_tests : BaseTest
    {
        [Test]
        public void Category_should_be_persisted()
        {
            var categoryRepo = Resolve<CategoryRepository>();

            var user = new User {Name = "Some user"};
            Resolve<UserRepository>().Create(user);
            
            var category = new Category("Sports") {Creator = user};
            categoryRepo.Create(category);

            var categoryFromDb = categoryRepo.GetAll().First();
            Assert.That(categoryFromDb.Name, Is.EqualTo("Sports"));
        }

    }
}

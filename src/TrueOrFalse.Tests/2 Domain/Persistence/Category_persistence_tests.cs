using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Category_persistence_tests : BaseTest
    {
        [Test]
        public void Category_should_be_persisted()
        {
            var categoryRepository = Resolve<CategoryRepository>();

            var category = new Category("Sports");
            category.SubCategories.Add(new SubCategory("Type"));
            category.SubCategories[0].Items.Add(new SubCategoryItem("Swimming"));
            category.SubCategories[0].Items.Add(new SubCategoryItem("Soccer"));
            category.SubCategories[0].Items.Add(new SubCategoryItem("Tennis"));

            categoryRepository.Create(category);

            var categoryFromDb = categoryRepository.GetAll().First();
            Assert.That(categoryFromDb.SubCategories.Count, Is.EqualTo(1));
            Assert.That(categoryFromDb.SubCategories[0].Items.Count, Is.EqualTo(3));
            Assert.That(categoryFromDb.SubCategories[0].Items[0].Name, Is.EqualTo("Swimming"));
        }

    }
}

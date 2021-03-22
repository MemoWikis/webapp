using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class DeepCloneTests : BaseTest
    {
        [Test]
        public void Should_clone_category_with_circular_relations()
        {
            //Crate to object with circular reference
            var categories = ContextCategory.New().Add("Root").Add("A").Persist().All;
            var categoryRoot = categories.ByName("Root");
            var categoryA = categories.ByName("A"); 
            
            categoryRoot.CategoryRelations = new List<CategoryRelation> {
                new CategoryRelation {Category = categoryRoot, RelatedCategory = categoryA, CategoryRelationType = CategoryRelationType.IsChildCategoryOf},
            };

            Assert.That(categoryRoot.DeepClone(), Is.Not.Null);

            categoryA.CategoryRelations = new List<CategoryRelation> {
                new CategoryRelation {Category = categoryA, RelatedCategory = categoryRoot, CategoryRelationType = CategoryRelationType.IsChildCategoryOf},
            };

            //Pre cloning, check that circular reference exists
            Assert.That(categoryRoot.ParentCategories().First().Name, Is.EqualTo("A"));
            Assert.That(categoryA.ParentCategories().First().Name, Is.EqualTo("Root"));
            Assert.That(categoryA.ParentCategories().First().ParentCategories().First().Name, Is.EqualTo("A"));


            //Clone
            var cloneA = categoryA.DeepClone();
            Assert.That(cloneA.ParentCategories().First().Name, Is.EqualTo("Root"));
            Assert.That(cloneA.ParentCategories().First().ParentCategories().First().Name, Is.EqualTo("A"));

        }

        [Test]
        public void Should_clone_category_from_db()
        {
            //test case 3 deepclone
            ContextCategory.New().AddCaseThreeToCache();
            RecycleContainer();

            var categoriesThird = Sl.CategoryRepo.GetAll();
            RecycleContainer();
            foreach (var category in categoriesThird)
            {
               var categoryCloned = category.DeepClone();
               Assert.That(categoryCloned, Is.Not.Null);
            }
        }
    }
}
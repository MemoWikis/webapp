
using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests._2_Domain.Category
{
    class Edit_category_tests : BaseTest
    {
        [Test]
        public void AddParent()
        {
            var categoryRepo = Sl.CategoryRepo; 
            var context = ContextCategory.New();
            var all =  context.Add("A").Add("B").Add("C").Persist().All;
            var editCategoryController = new EditCategoryController(categoryRepo);


            var child = all.ByName("C");
            var parent = all.ByName("A");
            editCategoryController.AddChild(child.Id, parent.Id);

            // Test Database
            parent = categoryRepo.GetByName("A").First();
            var hasParentCorrectRelation =
                TestHelper.hasRelation(parent, parent.Id, child.Id, CategoryRelationType.IncludesContentOf); 
            Assert.That(hasParentCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            child = categoryRepo.GetByName("C").First();
            var hasChildrenCorrectRelations =
                TestHelper.hasRelation(child, child.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(hasChildrenCorrectRelations, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            //TestEntityCache
            var childEntityCache = EntityCache.GetByName("C").First(); 
            hasChildrenCorrectRelations =
                TestHelper.hasRelation(childEntityCache, child.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(hasChildrenCorrectRelations, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

           var parentEntityCache = categoryRepo.GetByName("A").First();
             hasParentCorrectRelation =
                TestHelper.hasRelation(parentEntityCache, parent.Id, child.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(hasParentCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));
        }
    }
}

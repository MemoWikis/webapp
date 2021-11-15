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
            var all = context.Add("A").Add("B").Add("C").Add("D").Persist().All;
            EntityCache.Init();
            var editCategoryController = new EditCategoryController(categoryRepo);


            var child = all.ByName("D");
            var parent = all.ByName("B");

            var user = ContextUser.New().Add("User").Persist().All[0];
            Sl.SessionUser.Login(user);

            editCategoryController.AddChild(child.Id, parent.Id);

            // Test Database
            parent = categoryRepo.GetByName("B").First();
            var parentHasCorrectRelation =
                TestHelper.hasRelation(parent, parent.Id, child.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(parentHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            child = categoryRepo.GetByName("D").First();
            var childHasCorrectRelation =
                TestHelper.hasRelation(child, child.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            //TestEntityCache
            var childEntityCache = EntityCache.GetByName("D").First();
            childHasCorrectRelation =
                TestHelper.hasRelation(childEntityCache, child.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            var parentEntityCache = categoryRepo.GetByName("B").First();
            parentHasCorrectRelation =
                TestHelper.hasRelation(parentEntityCache, parent.Id, child.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(parentHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddParentForRootCategory()
        {
            var categoryRepo = Sl.CategoryRepo;
            var context = ContextCategory.New();
            var all = context.Add("A").Add("B").Add("C").Add("D").Persist().All;
            EntityCache.Init();
            var editCategoryController = new EditCategoryController(categoryRepo);


            var childC = all.ByName("C");
            var parent = all.ByName("A");

            var contextUser = ContextUser.New();
            var admin = contextUser.Add("admin").Persist().All[0];
            var user = contextUser.Add("user").Persist().All[1];


            admin.IsInstallationAdmin = true;
            Sl.SessionUser.Login(admin);

            editCategoryController.AddChild(childC.Id, parent.Id);

            // Test Database
            parent = categoryRepo.GetByName("A").First();
            var parentHasRelationToC =
                TestHelper.hasRelation(parent, parent.Id, childC.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(parentHasRelationToC, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            childC = categoryRepo.GetByName("C").First();
            var childHasCorrectRelation =
                TestHelper.hasRelation(childC, childC.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            //TestEntityCache
            var childCEntityCache = EntityCache.GetByName("C").First();
            childHasCorrectRelation =
                TestHelper.hasRelation(childCEntityCache, childC.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            var parentEntityCache = categoryRepo.GetByName("A").First();
            parentHasRelationToC =
                TestHelper.hasRelation(parentEntityCache, parent.Id, childC.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(parentHasRelationToC, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            Sl.SessionUser.Logout();

            var childD = all.ByName("D");

            Sl.SessionUser.Login(user);

            editCategoryController.AddChild(childD.Id, parent.Id);

            // Test Database
            parent = categoryRepo.GetByName("A").First();
            var parentHasRelationToD =
                TestHelper.hasRelation(parent, parent.Id, childD.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(parentHasRelationToD, Is.EqualTo(false));

            childD = categoryRepo.GetByName("D").First();
            var childDHasRelation =
                TestHelper.hasRelation(childD, childD.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(childDHasRelation, Is.EqualTo(false));

            //TestEntityCache
            var childEntityCache = EntityCache.GetByName("D").First();
            childHasCorrectRelation =
                TestHelper.hasRelation(childEntityCache, childD.Id, parent.Id, CategoryRelationType.IsChildOf);
            Assert.That(childDHasRelation, Is.EqualTo(false));

            parentHasRelationToD =
                TestHelper.hasRelation(parentEntityCache, parent.Id, childD.Id, CategoryRelationType.IncludesContentOf);
            Assert.That(parentHasRelationToD, Is.EqualTo(false));

        }
    }
}
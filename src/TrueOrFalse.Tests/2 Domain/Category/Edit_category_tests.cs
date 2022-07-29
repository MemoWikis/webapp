using System.Collections.Generic;
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
            SessionUser.Login(user);

            editCategoryController.AddChild(child.Id, parent.Id);

            //TestEntityCache
            var childEntityCache = EntityCache.GetCategoryByName("D").First();
            var childHasCorrectRelation =
                TestHelper.HasRelation(childEntityCache, child.Id, parent.Id);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(childEntityCache.CategoryRelations.Count, Is.EqualTo(1));

            var parentEntityCache = EntityCache.GetCategoryByName("B").First();
            var parentHasCorrectRelation =
                TestHelper.HasRelation(parentEntityCache, parent.Id, child.Id);
            Assert.That(parentHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parentEntityCache.CategoryRelations.Count, Is.EqualTo(1));


            // Test Database
            parent = categoryRepo.GetByName("B").First();
            parentHasCorrectRelation =
               TestHelper.HasRelation(parent, parent.Id, child.Id);
            Assert.That(parentHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            child = categoryRepo.GetByName("D").First();
            childHasCorrectRelation =
               TestHelper.HasRelation(child, child.Id, parent.Id);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
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
            SessionUser.Login(admin);

            editCategoryController.AddChild(childC.Id, parent.Id);

            // Test Database
            parent = categoryRepo.GetByName("A").First();
            var parentHasRelationToC =
                TestHelper.HasRelation(parent, parent.Id, childC.Id);
            Assert.That(parentHasRelationToC, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            childC = categoryRepo.GetByName("C").First();
            var childHasCorrectRelation =
                TestHelper.HasRelation(childC, childC.Id, parent.Id);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            //TestEntityCache
            var childCEntityCache = EntityCache.GetCategoryByName("C").First();
            childHasCorrectRelation =
                TestHelper.HasRelation(childCEntityCache, childC.Id, parent.Id);
            Assert.That(childHasCorrectRelation, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            var parentEntityCache = categoryRepo.GetByName("A").First();
            parentHasRelationToC =
                TestHelper.HasRelation(parentEntityCache, parent.Id, childC.Id);
            Assert.That(parentHasRelationToC, Is.EqualTo(true));
            Assert.That(parent.CategoryRelations.Count, Is.EqualTo(1));

            SessionUser.Logout();

            var childD = all.ByName("D");

            SessionUser.Login(user);

            editCategoryController.AddChild(childD.Id, parent.Id);

            // Test Database
            parent = categoryRepo.GetByName("A").First();
            var parentHasRelationToD =
                TestHelper.HasRelation(parent, parent.Id, childD.Id);
            Assert.That(parentHasRelationToD, Is.EqualTo(false));

            childD = categoryRepo.GetByName("D").First();
            var childDHasRelation =
                TestHelper.HasRelation(childD, childD.Id, parent.Id);
            Assert.That(childDHasRelation, Is.EqualTo(false));

            //TestEntityCache
            var childEntityCache = EntityCache.GetCategoryByName("D").First();
            childHasCorrectRelation =
                TestHelper.HasRelation(childEntityCache, childD.Id, parent.Id);
            Assert.That(childDHasRelation, Is.EqualTo(false));

            parentHasRelationToD =
                TestHelper.HasRelation(parentEntityCache, parent.Id, childD.Id);
            Assert.That(parentHasRelationToD, Is.EqualTo(false));

        }

        [Test]
        public void Admin_Can_Set_Any_Category_To_Private()
        {
            var categoryRepo = Sl.CategoryRepo;
            var contextCategory = ContextCategory.New();
            var contextUser = ContextUser.New();
            var contextQuestion = ContextQuestion.New();
            contextCategory.Add("Root").Add("B").Add("C").Add("D").Persist();

            var users = contextUser.Add("Admin").Add("NonAdmin").Add("publicWiki").Add("questionInWuwi").Persist(true, contextCategory);
            var admin = contextUser.All[0];
            var nonAdmin = contextUser.All[1];
            var publicWikiUser = contextUser.All[2];
            var questionInWuwiUser = contextUser.All[3];
            var publicWiki = contextCategory.Persist().All.ByName("publicWikis Startseite");
            contextCategory.Add("publicWikis Child", parent: publicWiki, creator: publicWikiUser, id: 8).Persist();

            var questionInWuwiWiki = contextCategory.All.ByName("questionInWuwis Startseite");


            var editCategoryController = new EditCategoryController(categoryRepo);
            var categoryValuationRepo = Sl.CategoryValuationRepo;

            EntityCache.Init();

            var question = contextQuestion.AddQuestion(creator: questionInWuwiUser).Persist().All[0];
            question.Categories.Add(questionInWuwiWiki);
            SessionUser.Login(admin);

            var wuwiUsers = new List<User>();
            var i = 0;
            while (i < 10)
            {
                var name = "PinUser" + (i + 1);
                var wuwiUser = contextUser.Add(name).Persist(false).All.First(u => u.Name == name);
                wuwiUsers.Add(wuwiUser);
                i++;
            }

            QuestionInKnowledge.Pin(question.Id, wuwiUsers.First());


            editCategoryController.SetCategoryToPrivate(publicWiki.Id);
            editCategoryController.SetCategoryToPrivate(questionInWuwiWiki.Id);

            var publicWikiCategory = categoryRepo.GetById(publicWiki.Id);
            var questionInWuwiWikiCategory = categoryRepo.GetById(publicWiki.Id);
            Assert.That(publicWikiCategory.Visibility, Is.EqualTo(CategoryVisibility.Owner));
            Assert.That(questionInWuwiWikiCategory.Visibility, Is.EqualTo(CategoryVisibility.Owner));

            var pinUserHasNoPublicWikiInWuwi = Sl.UserRepo.GetByName("PinUser1");
            var publicWikiIsWishknowledgeForPinUser1 = categoryValuationRepo.GetByUser(pinUserHasNoPublicWikiInWuwi.Id).First(v => v.CategoryId == publicWiki.Id && pinUserHasNoPublicWikiInWuwi.Id == v.UserId).IsInWishKnowledge();

            Assert.That(publicWikiIsWishknowledgeForPinUser1, Is.EqualTo(false));
        }
    }
}
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using BDDish.Model;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;
using TrueOrFalse.Tests;

class User_entity_cache_tests : BaseTest
{
    [Test]
    public void Should_return_correct_categories()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();

        SessionUser.Login(user);
        UserEntityCache.Init(user.Id);
        var userEntityCacheCategories = UserEntityCache.GetAllCategoriesAsDictionary(user.Id).Values.ToList();
        var entityCacheCategories = EntityCache.GetAllCategories().ToList();

        // entityCacheCategories is uncut case and userEntityCacheCategories is cut case  https://app.diagrams.net/#G1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6

        //EntityCache
        Assert.That(
            entityCacheCategories.ByName("I").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("I").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "E").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("I").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "G").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("H").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("G").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("F").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("E").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("D").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "B").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("B").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("C").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("C").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X2").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("C").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X1").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("X2").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("X").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("X1").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(
            entityCacheCategories.ByName("X3").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));

        //userEntityCache
        Assert.That(
            userEntityCacheCategories.ByName("I").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("I").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "G").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("I").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X3").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("G").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("G").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X3").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("F").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("F").CategoryRelations
                .Where(cr => EntityCache.GetCategory(cr.RelatedCategoryId).Name == "X3").Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("X").CategoryRelations.Where(cr =>
                EntityCache.GetCategory(cr.RelatedCategoryId).Name ==
                EntityCache.GetCategory(user.StartTopicId).Name).Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("X3").CategoryRelations.Where(cr =>
                EntityCache.GetCategory(cr.RelatedCategoryId).Name ==
                EntityCache.GetCategory(user.StartTopicId).Name).Count, Is.EqualTo(1));
        Assert.That(
            userEntityCacheCategories.ByName("B").CategoryRelations.Where(cr =>
                EntityCache.GetCategory(cr.RelatedCategoryId).Name ==
                EntityCache.GetCategory(user.StartTopicId).Name).Count, Is.EqualTo(1));
    }

    [Test]
    public void Give_correct_number_of_cache_items_case_three()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        SessionUser.Login(user);
        UserEntityCache.Init(user.Id);
        Assert.That(UserEntityCache.GetAllCategoriesAsDictionary(user.Id).Values.ToList().Count, Is.EqualTo(7));
        Assert.That(EntityCache.GetAllCategories().Count, Is.EqualTo(14));
    }

    [Test]
    public void Give_correct_number_of_cache_items_case_two()
    {
        ContextCategory.New().AddCaseTwoToCache();
        EntityCache.Init();
        var user = SessionUser.User;
        UserEntityCache.Init(user.Id);
        Assert.That(UserEntityCache.GetAllCategoriesAsDictionary(user.Id).Values.ToList().Count, Is.EqualTo(5));
        Assert.That(EntityCache.GetAllCategories().Count, Is.EqualTo(10));
    }

    [Test]
    public void Give_correct_children()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        SessionUser.Login(user);
        UserEntityCache.Init(user.Id);
        var children = UserEntityCache.GetChildren(user.StartTopicId, user.Id);

        Assert.That(children.Where(c => c.Name == "B").Count(), Is.EqualTo(1));
        Assert.That(children.Where(c => c.Name == "X").Count(), Is.EqualTo(1));
        Assert.That(children.Where(c => c.Name == "X3").Count(), Is.EqualTo(1));
        Assert.That(children.Count, Is.EqualTo(3));
        Assert.That(EntityCache.GetAllCategories().Count, Is.EqualTo(14));
    }

    [Test]
    public void Test_next_parent_in_wishKnowledge()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        SessionUser.Login(user);
        UserEntityCache.Init(user.Id);
        ContextCategory.New().Add("noParent").Persist();
        var noParent = EntityCache.GetAllCategories().ByName("noParent");
        CategoryInKnowledge.Pin(noParent.Id, user);

        EntityCache.Init();

        var e = EntityCache.GetAllCategories().ByName("E");
        var d = EntityCache.GetAllCategories().ByName("D");
        var c = EntityCache.GetAllCategories().ByName("C");
        var x1 = EntityCache.GetAllCategories().ByName("X1");
        var h = EntityCache.GetAllCategories().ByName("H");

        var nextParetFromE = UserEntityCache.GetNextParentInWishKnowledge(e.Id);
        var nextParetFromD = UserEntityCache.GetNextParentInWishKnowledge(d.Id);
        var nextParetFromC = UserEntityCache.GetNextParentInWishKnowledge(c.Id);
        var nextParetFromX1 = UserEntityCache.GetNextParentInWishKnowledge(x1.Id);
        var nextParetFromH = UserEntityCache.GetNextParentInWishKnowledge(x1.Id);
        var nextParentFromNoParent = UserEntityCache.GetNextParentInWishKnowledge(noParent.Id);

        Assert.That(nextParetFromX1.Name, Is.EqualTo("X3"));
        Assert.That(nextParetFromE.Name, Is.EqualTo("X"));
        Assert.That(nextParetFromD.Name, Is.EqualTo("B"));
        Assert.That(nextParetFromC.Name, Is.EqualTo("X"));
        Assert.That(nextParetFromH.Name, Is.EqualTo("X3"));
        Assert.That(nextParentFromNoParent.Name,
            Is.EqualTo(UserEntityCache.GetCategoryWhenNotAvalaibleThenGetNextParent(user.StartTopicId, user.Id).Name));
    }

    [Test]
    public void Test_init_for_user_entity_cache_change_name()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        SessionUser.Login(user);
        UserEntityCache.Init(user.Id);

        var cat = EntityCache.GetCategory(user.StartTopicId);
        cat.Name = "Daniel";
        UserEntityCache.ReInitAllActiveCategoryCaches();

        Assert.That(UserEntityCache.GetCategory(user.Id, user.StartTopicId).Name, Is.EqualTo("Daniel"));
        Assert.That(UserEntityCache.GetAllCategoriesAsDictionary(2).Count, Is.EqualTo(7));
    }

    [Test]
    public void Test_change_for_all_user_entity_caches()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        SessionUser.Login(user);
        UserEntityCache.Init(user.Id);
        var cat = EntityCache.GetAllCategories().ByName("X3");
        cat.Name = "Daniel";
        UserEntityCache.ChangeCategoryInUserEntityCaches(cat);
        Assert.That(UserEntityCache.GetCategory(user.Id, cat.Id).Name, Is.EqualTo("Daniel"));
        Assert.That(UserEntityCache.GetAllCategoriesAsDictionary(2).Count, Is.EqualTo(7));

        user = ContextUser.New().Add("Daniel").Persist(true).All.First();
        SessionUser.Login(user);
        CategoryInKnowledge.Pin(cat.Id, user);
        UserEntityCache.Init();

        cat = UserEntityCache.GetCategory(user.Id, cat.Id);
        Assert.That(UserEntityCache.GetAllCategories(user.Id).Count(), Is.EqualTo(2)); // is RootKategorie
        Assert.That(cat.Name, Is.EqualTo("Daniel"));
    }

    [Test]
    public void Test_delete_category()
    {
        var A = ContextCategory.New().Add("A").Persist().All.First();
        var B = ContextCategory.New().Add("B", parent: A).Persist().All.ByName("B");
        ContextCategory.New().Add("C", parent: B).Persist();

        var allCategories = EntityCache.GetAllCategories();

        var user = ContextUser.New().Add("Daniel").Persist(true).All.First();
        SessionUser.Login(user);

        foreach (var VARIABLE in allCategories)
        {
            CategoryInKnowledge.Pin(VARIABLE.Id, user);
        }

        UserEntityCache.Init();
        var BCache = allCategories.ByName("B");
        var C = allCategories.ByName("C");


        ModifyRelationsUserEntityCache.DeleteFromAllParents(BCache);
        ModifyRelationsUserEntityCache.DeleteFromAllParents(C);

        UserEntityCache.DeleteCategory(B.Id);
        UserEntityCache.DeleteCategory(C.Id);

        Assert.That(UserEntityCache.GetAllCategories(user.Id).Contains(BCache), Is.EqualTo(false));
        Assert.That(UserEntityCache.GetAllCategories(user.Id).Contains(C), Is.EqualTo(false));
        Assert.That(UserEntityCache.GetByName(user.Id, "B").Count(), Is.EqualTo(0));
        Assert.That(UserEntityCache.GetByName(user.Id, "C").Count(), Is.EqualTo(0));
    }

    [Test]
    public void Get_all_parents_from_entity_cache()
    {
        ContextCategory.New().AddCaseThreeToCache();
        var parentNames = GraphService.GetAllParentsFromEntityCache(
                EntityCache.GetCategoryByName("I")
                    .Where(c => c.Name == "I")
                    .First().Id)
            .Select(c => c.Name);

        Assert.That(parentNames.Contains("G"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("X3"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("X"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("C"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("E"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("G"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("A"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("X2"), Is.EqualTo(true));
        Assert.That(parentNames.Count(), Is.EqualTo(8));
    }

    [Test]
    public void Create_category()
    {
        var cateContext = ContextCategory.New();
        var user = cateContext.AddCaseThreeToCache();
        EntityCache.Init();
        SessionUser.Login(user);
        UserCache.GetItem(user.Id).IsFiltered = true;
        UserEntityCache.Init();
        cateContext.Add("New", parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();

        var newCat = UserEntityCache.GetByName(user.Id, "New").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId, Is.EqualTo(user.StartTopicId));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetCategoryByName("New").First().Id));

        Assert.That(UserEntityCache.GetByName(user.Id, "New").First().CategoryRelations.Count, Is.EqualTo(1));

        newCat = EntityCache.GetCategoryByName("New").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId,
            Is.EqualTo(EntityCache.GetCategoryByName("X2").First().Id));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetCategoryByName("New").First().Id));

        Assert.That(UserEntityCache.GetByName(user.Id, "New").First().CategoryRelations.Count, Is.EqualTo(1));

        UserCache.GetItem(user.Id).IsFiltered = false;
        cateContext.Add("New1", parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();

        newCat = UserEntityCache.GetByName(user.Id, "New1").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId, Is.EqualTo(user.StartTopicId));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetCategoryByName("New1").First().Id));

        Assert.That(UserEntityCache.GetByName(user.Id, "New1").First().CategoryRelations.Count, Is.EqualTo(1));

        var hasChildrenInUserCachedData = UserEntityCache.GetCategory(user.Id, user.StartTopicId)
            .CachedData.ChildrenIds
            .ElementAt(newCat.Id) != -1;

        var hasChildrenInEntityCachedData = EntityCache.GetCategoryByName("X2")
            .SelectMany(cCI => cCI.CachedData.ChildrenIds.Select(id => id)).ToList().IndexOf(newCat.Id) != -1;

        Assert.That(hasChildrenInUserCachedData, Is.EqualTo(true));
        Assert.That(hasChildrenInEntityCachedData, Is.EqualTo(true));

        newCat = EntityCache.GetCategoryByName("New1").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId,
            Is.EqualTo(EntityCache.GetCategoryByName("X2").First().Id));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetCategoryByName("New1").First().Id));

        Assert.That(UserEntityCache.GetByName(user.Id, "New1").First().CategoryRelations.Count, Is.EqualTo(1));

        hasChildrenInUserCachedData = UserEntityCache.GetCategory(user.Id, user.StartTopicId)
            .CachedData
            .ChildrenIds
            .ElementAt(newCat.Id) != -1;

        hasChildrenInEntityCachedData = EntityCache.GetCategoryByName("X2")
            .First()
            .CachedData
            .ChildrenIds
            .ElementAt(newCat.Id) != -1;

        Assert.That(hasChildrenInUserCachedData, Is.EqualTo(true));
        Assert.That(hasChildrenInEntityCachedData, Is.EqualTo(true));
    }

    [Test]
    public void Create_category_test_case_2_without_my_world()
    {
        var context = ContextCategory.New();
        context.Add("1").Persist();
        context.Add("2", parent: context.All.ByName("1")).Persist();
        context.Add("3", parent: context.All.ByName("2")).Persist();
        context.Add("4", parent: context.All.ByName("3")).Persist();
        context.Add("5", parent: context.All.ByName("4")).Persist();

        Assert.That(EntityCache.GetCategoryByName("1").First().CategoryRelations.Count, Is.EqualTo(4));
    }

    [Test]
    public void Delete_category()
    {
        var context = ContextCategory.New();
        var contextUser = ContextUser.New();
        var user1 = context.AddCaseThreeToCache(contextUser: contextUser);
        var user = context.AddCaseThreeToCache(contextUser: contextUser);

        EntityCache.Init();
        UserEntityCache.GetAllCaches(); // This expression causes side effects and will not be evaluated	

        context.Add("New", creator: user, parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();
        context.Add("New1", creator: user, parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();

        CategoryInKnowledge.Pin(EntityCache.GetCategoryByName("New").First().Id, user1);
        CategoryInKnowledge.Pin(EntityCache.GetCategoryByName("New1").First().Id, user1);

        var categoryNew = Sl.CategoryRepo.GetByName("New").First();

        Resolve<CategoryDeleter>().Run(categoryNew, true);

        var userCaches = UserEntityCache.GetAllCaches();
        var hasDeletedIdInRelations = false;
        var hasDeletedIdInCachedData = false;
        foreach (var userCachesValue in userCaches.Values)
        {
            if (userCachesValue.Values
                    .SelectMany(cci => cci.CategoryRelations
                        .Select(cr => cr.RelatedCategoryId))
                    .ToList()
                    .IndexOf(categoryNew.Id) != -1)
                hasDeletedIdInRelations = true;

            if (userCachesValue.Values
                    .SelectMany(cCI => cCI.CachedData.ChildrenIds
                        .Select(id => id))
                    .ToList()
                    .IndexOf(categoryNew.Id) != -1)
                hasDeletedIdInCachedData = true;
        }

        Assert.That(hasDeletedIdInRelations, Is.EqualTo(false));
        Assert.That(hasDeletedIdInCachedData, Is.EqualTo(false));

        var categoryNew1 = EntityCache.GetCategoryByName("New1").First();
    }

    [Test]
    public void Update_category()
    {
        var cateContext = ContextCategory.New();
        var user = cateContext.AddCaseThreeToCache();
        var all = cateContext.All;


        var category = Sl.CategoryRepo.GetByName("B").First();
        category.CategoryRelations.Clear();
        category.CategoryRelations.Add(new CategoryRelation
        {
            RelatedCategory = Sl.CategoryRepo.GetByName("X3").First(),
            Category = category
        });
        new CacheUpdater(category);

        var c = Sl.CategoryRepo.GetByName("B");

        Sl.CategoryRepo.Update(category);
    }

    ///  <image url = "https://share-your-photo.com/img/7c617b0eb7.png" scale="1" />
    [Test]
    public void Get_correct_relations_from_another_startsite()
    {
        var user1 = ContextUser.New().Add("user1").Persist(true).All.First();
        var user2 = ContextUser.New().Add("user2").Persist(true).All.First();
        EntityCache.GetCategory(user1.StartTopicId);
        var userStartTopic2 = Sl.CategoryRepo.GetById(user2.StartTopicId);

        var categoryA = ContextCategory.New(false).Add("A", creator: user1).Persist().All.First();
        var categoryB = ContextCategory.New(false).Add("B", creator: user1).Persist().All.First();
        var categoryC = ContextCategory.New(false).Add("C", creator: user1).Persist().All.First();

        categoryC.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryC,
            RelatedCategory = categoryA
        });

        categoryB.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryB,
            RelatedCategory = categoryA
        });

        categoryB.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryB,
            RelatedCategory = userStartTopic2
        });

        CategoryInKnowledge.Pin(categoryB.Id, user1);
        CategoryInKnowledge.Pin(categoryC.Id, user1);
        EntityCache.Init();
        UserCache.GetItem(user1.Id).IsFiltered = true;
        SessionUser.Login(user1);

        Assert.That(UserEntityCache.GetCategory(user1.Id, categoryB.Id).CategoryRelations.First().RelatedCategoryId,
            Is.EqualTo(user1.StartTopicId));
        Assert.That(UserEntityCache.GetCategory(user1.Id, categoryC.Id).CategoryRelations.First().RelatedCategoryId,
            Is.EqualTo(user1.StartTopicId));
    }

    ///  <image url = "https://share-your-photo.com/img/8278fabf18.png" scale="1" />
    [Test]
    public void Get_correct_relations_with_another_startsite()
    {
        var user1 = ContextUser.New().Add("user1").Persist(true).All.First();
        var user2 = ContextUser.New().Add("user2").Persist(true).All.First();
        var userStartTopic1 = EntityCache.GetCategory(user1.StartTopicId);
        var userStartTopic2 = Sl.CategoryRepo.GetById(user2.StartTopicId);

        var categoryA = ContextCategory.New(false).Add("A", creator: user1).Persist().All.First();
        var categoryB = ContextCategory.New(false).Add("B", creator: user1).Persist().All.First();
        var categoryC = ContextCategory.New(false).Add("C", creator: user1).Persist().All.First();

        categoryC.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryC,
            RelatedCategory = categoryA
        });

        categoryB.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryB,
            RelatedCategory = categoryA
        });

        categoryB.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryB,
            RelatedCategory = userStartTopic2
        });

        CategoryInKnowledge.Pin(categoryB.Id, user1);
        CategoryInKnowledge.Pin(categoryC.Id, user1);
        CategoryInKnowledge.Pin(userStartTopic2.Id, user1);

        EntityCache.Init();
        UserCache.GetItem(user1.Id).IsFiltered = true;
        SessionUser.Login(user1);

        var categoryBParentFromUserEntityCache = UserEntityCache.GetCategory(user1.Id, categoryB.Id).CategoryRelations
            .First().RelatedCategoryId;
        var categoryBAllRelations = UserEntityCache.GetCategory(user1.Id, categoryB.Id).CategoryRelations.Count;
        Assert.That(categoryBParentFromUserEntityCache, Is.EqualTo(user2.StartTopicId));
        Assert.That(categoryBAllRelations, Is.EqualTo(1));


        var relatedChildrenIdsInUser2StartTopic = UserEntityCache.GetCategory(user1.Id, userStartTopic2.Id)
            .CategoryRelations
            .Select(cr => cr.RelatedCategoryId);
        var allRelationsInUser2StartTopic = UserEntityCache.GetCategory(user1.Id, userStartTopic2.Id)
            .CategoryRelations;
        Assert.That(relatedChildrenIdsInUser2StartTopic.Count(), Is.EqualTo(1));
        Assert.That(relatedChildrenIdsInUser2StartTopic.First(), Is.EqualTo(user1.StartTopicId));
        Assert.That(allRelationsInUser2StartTopic.Count, Is.EqualTo(2));

        var allRelationsInCategoryC = UserEntityCache.GetCategory(user1.Id, categoryC.Id)
            .CategoryRelations;
        Assert.That(allRelationsInCategoryC.First().RelatedCategoryId, Is.EqualTo(user1.StartTopicId));
        Assert.That(allRelationsInCategoryC.Count, Is.EqualTo(1));
    }


    ///  <image url = "https://share-your-photo.com/img/b1a0487f97.png" scale="1" />
    [Test]
    public void Get_correct_relations_with_another_startsite1()
    {
        var user1 = ContextUser.New().Add("user1").Persist(true).All.First();
        var user2 = ContextUser.New().Add("user2").Persist(true).All.First();
        var userStartTopic1 = EntityCache.GetCategory(user1.StartTopicId);
        var userStartTopic2 = Sl.CategoryRepo.GetById(user2.StartTopicId);

        var categoryA = ContextCategory.New(false).Add("A", creator: user1).Persist().All.First();
        var categoryB = ContextCategory.New(false).Add("B", creator: user1).Persist().All.First();
        var categoryC = ContextCategory.New(false).Add("C", creator: user1).Persist().All.First();

        categoryC.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryC,
            RelatedCategory = categoryA
        });

        categoryC.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryC,
            RelatedCategory = categoryB
        });

        categoryB.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryB,
            RelatedCategory = categoryA
        });

        categoryB.CategoryRelations.Add(new CategoryRelation
        {
            Category = categoryB,
            RelatedCategory = userStartTopic2
        });

        CategoryInKnowledge.Pin(categoryB.Id, user1);
        CategoryInKnowledge.Pin(categoryC.Id, user1);
        CategoryInKnowledge.Pin(userStartTopic2.Id, user1);

        EntityCache.Init();
        UserCache.GetItem(user1.Id).IsFiltered = true;
        SessionUser.Login(user1);

        var relationsC = UserEntityCache.GetCategory(user1.Id, categoryC.Id).CategoryRelations;
        Assert.That(relationsC.First().RelatedCategoryId, Is.EqualTo(categoryB.Id));
        Assert.That(relationsC.Count, Is.EqualTo(1));
    }
}
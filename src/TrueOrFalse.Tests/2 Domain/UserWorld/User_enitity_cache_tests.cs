using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TrueOrFalse.Tests;

class User_entity_cache_tests : BaseTest
{
    [Test]
    public void Should_return_correct_categories()
    {
        ContextCategory.New().AddCaseThreeToCache();
        var user = Sl.SessionUser.User;
       
        EntityCache.Init();
        var userEntityCacheCategories = UserEntityCache.GetCategories(user.Id).Values.ToList();
        var entityCacheCategories = EntityCache.GetAllCategories().ToList();

        // entityCacheCategories is uncut case and userEntityCacheCategoriess is cut case  https://app.diagrams.net/#G1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6

        //EntityCache
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "E").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "G").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("H").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("G").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("F").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("E").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("D").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "B").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("B").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X2").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X1").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X2").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X1").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X3").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));

        //userEntityCache
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "G").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("G").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("G").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("F").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("F").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("X").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("X3").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("B").CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A").Count, Is.EqualTo(1));

    }

    [Test]
    public void Give_correct_number_of_cache_items()
    {
        ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        var user = Sl.SessionUser.User;
        Assert.That(UserEntityCache.GetCategories(user.Id).Values.ToList().Count, Is.EqualTo(7));

        ContextCategory.New(false).AddCaseTwoToCache();
        Thread.Sleep(100);
        EntityCache.Init();
        user = Sl.SessionUser.User;
        Assert.That(UserEntityCache.GetCategories(user.Id).Values.ToList().Count, Is.EqualTo(5));
    }

    [Test]
    public void Give_correct_children()
    {
        ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        var user = Sl.SessionUser.User;
      var children =   UserEntityCache.GetChildren(1, user.Id); 

        Assert.That(children.Where(c => c.Name == "B").Count(), Is.EqualTo(1));
        Assert.That(children.Where(c => c.Name == "X").Count(), Is.EqualTo(1));
        Assert.That(children.Where(c => c.Name == "X3").Count(), Is.EqualTo(1));
        Assert.That(children.Count, Is.EqualTo(3));

    }

    [Test]
    public void Test_next_parent_in_wishknowledge()
    {
      var user = ContextCategory.New().AddCaseThreeToCache();
      ContextCategory.New().Add("noParent").Persist();
      var noParent = EntityCache.GetAllCategories().ByName("noParent");
      CategoryInKnowledge.Pin(noParent.Id, user);

        EntityCache.Init();

        var e = EntityCache.GetAllCategories().ByName("E");
        var d = EntityCache.GetAllCategories().ByName("D");
        var c = EntityCache.GetAllCategories().ByName("C");
        var x1 = EntityCache.GetAllCategories().ByName("X1");
        var h = EntityCache.GetAllCategories().ByName("H");

        var nextParetFromE = UserEntityCache.GetNextParentInWishknowledge(e.Id); 
        var nextParetFromD = UserEntityCache.GetNextParentInWishknowledge(d.Id); 
        var nextParetFromC = UserEntityCache.GetNextParentInWishknowledge(c.Id); 
        var nextParetFromX1 = UserEntityCache.GetNextParentInWishknowledge(x1.Id); 
        var nextParetFromH = UserEntityCache.GetNextParentInWishknowledge(x1.Id);
        var nextParentFromNoParent = UserEntityCache.GetNextParentInWishknowledge(noParent.Id);

        Assert.That(nextParetFromX1.Name, Is.EqualTo("X3"));
        Assert.That(nextParetFromE.Name, Is.EqualTo("X"));
        Assert.That(nextParetFromD.Name, Is.EqualTo("B"));
        Assert.That(nextParetFromC.Name, Is.EqualTo("X"));
        Assert.That(nextParetFromH.Name, Is.EqualTo("X3"));
        Assert.That(nextParentFromNoParent.Name, Is.EqualTo("A"));
    }

    [Test]
    public void Test_init_for_all_user_entity_caches_change_name()
    {
        ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();

        var cate = EntityCache.GetAllCategories().First();
        cate.Name = "Daniel";
        UserEntityCache.ReInitAllActiveCategoryCaches();


        Assert.That(UserEntityCache.GetCategories(2).First().Value.Name, Is.EqualTo("Daniel"));
        Assert.That(UserEntityCache.GetCategories(2).Count, Is.EqualTo(7));


        var user = ContextUser.New().Add("Daniel").Persist().All.First();
        Sl.SessionUser.Login(user);
        UserEntityCache.Init();

        var userEntityCacheAfterRenameForUser3 = UserEntityCache.GetCategories(3);
        Assert.That(userEntityCacheAfterRenameForUser3.Count, Is.EqualTo(1)); // is RootKategorie
        Assert.That(UserEntityCache.GetCategories(3).First().Value.Name, Is.EqualTo("Daniel"));
    }

    [Test]
    public void Test_change_for_all_user_entity_caches()
    {
        ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();

        var cate = EntityCache.GetAllCategories().First();
        cate.Name = "Daniel";
        UserEntityCache.ChangeCategoryInUserEntityCaches(cate);

        Assert.That(UserEntityCache.GetCategories(2).First().Value.Name, Is.EqualTo("Daniel"));
        Assert.That(UserEntityCache.GetCategories(2).Count, Is.EqualTo(7));

        var user = ContextUser.New().Add("Daniel").Persist().All.First();
        Sl.SessionUser.Login(user);
        UserEntityCache.Init();
         
        var userEntityCacheAfterRenameForUser3 = UserEntityCache.GetCategories(3);
        Assert.That(userEntityCacheAfterRenameForUser3.Count, Is.EqualTo(1)); // is RootKategorie
        Assert.That(UserEntityCache.GetCategories(3).First().Value.Name, Is.EqualTo("Daniel"));
    }


    [Test]
    public void Test_delete_category()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();

        CategoryInKnowledge.Pin(EntityCache.GetAllCategories().ByName("E").Id, Sl.UserRepo.GetById(2));
        Sl.CategoryRepo.Delete(Sl.CategoryRepo.GetByName("E").First());


        var userEntityCacheAfterDeleteForUser2 = UserEntityCache.GetCategories(user.Id).Select(x => x.Value).ToList();
        var alleCategories = EntityCache.GetAllCategories();
        var categoryIdForA = alleCategories.ByName("A").Id; 


        Assert.That(userEntityCacheAfterDeleteForUser2.Count, Is.EqualTo(7));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("X").CategoryRelations.First().RelatedCategoryId, Is.EqualTo(categoryIdForA));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("X3").CategoryRelations.First().RelatedCategoryId, Is.EqualTo(categoryIdForA));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("B").CategoryRelations.First().RelatedCategoryId, Is.EqualTo(categoryIdForA));

        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("G").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("G").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("G").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A"), Is.EqualTo(0));

        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("F").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("F").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("F").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A"), Is.EqualTo(0));

        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "G"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "X3"), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "A"), Is.EqualTo(0));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Count(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "E"), Is.EqualTo(0));
    }
    [Test]
    public void Get_all_parents()
    {
        var user= ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        UserEntityCache.Init(user.Id);
        UserCache.GetItem(user.Id).IsFiltered = true; 

        var parentNames = GraphService.GetAllParentsFromEntityCache(
            UserEntityCache.GetAllCategories(user.Id)
                .Where(c => c.Name == "I").First().Id).Select(c => c.Name);

        Assert.That(parentNames.Contains("G"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("X3"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("X"), Is.EqualTo(true));
        Assert.That(parentNames.Contains("A"), Is.EqualTo(true));
        Assert.That(parentNames.Count(), Is.EqualTo(4)); 
    }

    [Test]
    public void Create_category()
    {
        var cateContext = ContextCategory.New();
        var user = cateContext.AddCaseThreeToCache();
        EntityCache.Init();

        UserCache.GetItem(user.Id).IsFiltered = true; 
        UserEntityCache.Init();
        cateContext.Add("New", parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();

        var newCat = UserEntityCache.GetByName(user.Id, "New").First(); 
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId, Is.EqualTo(RootCategory.RootCategoryId));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetByName("New").First().Id));
        Assert.That(UserEntityCache.GetByName(user.Id, "New").First().CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));
        Assert.That(UserEntityCache.GetByName(user.Id, "New").First().CategoryRelations.Count, Is.EqualTo(1));

        newCat = EntityCache.GetByName("New").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId, Is.EqualTo(EntityCache.GetByName("X2").First().Id));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetByName("New").First().Id));
        Assert.That(UserEntityCache.GetByName(user.Id, "New").First().CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));
        Assert.That(UserEntityCache.GetByName(user.Id, "New").First().CategoryRelations.Count, Is.EqualTo(1));

        UserCache.GetItem(user.Id).IsFiltered = false;
        cateContext.Add("New1", parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();

        newCat = UserEntityCache.GetByName(user.Id, "New1").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId, Is.EqualTo(RootCategory.RootCategoryId));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetByName("New1").First().Id));
        Assert.That(UserEntityCache.GetByName(user.Id, "New1").First().CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));
        Assert.That(UserEntityCache.GetByName(user.Id, "New1").First().CategoryRelations.Count, Is.EqualTo(1));

        var hasChildrenInUserCachedData = UserEntityCache.GetByName(user.Id, "A")
            .SelectMany(cCI => cCI.CachedData.ChildrenIds.Select(id => id)).ToList().IndexOf(newCat.Id) != -1;

        var hasChildrenInEntityCachedData = EntityCache.GetByName( "X2")
            .SelectMany(cCI => cCI.CachedData.ChildrenIds.Select(id => id)).ToList().IndexOf(newCat.Id) != -1;

        Assert.That(hasChildrenInUserCachedData, Is.EqualTo(true));
        Assert.That(hasChildrenInEntityCachedData, Is.EqualTo(true));

        newCat = EntityCache.GetByName("New1").First();
        Assert.That(newCat.CategoryRelations.First().RelatedCategoryId, Is.EqualTo(EntityCache.GetByName("X2").First().Id));
        Assert.That(newCat.CategoryRelations.First().CategoryId, Is.EqualTo(EntityCache.GetByName("New1").First().Id));
        Assert.That(UserEntityCache.GetByName(user.Id, "New1").First().CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));
        Assert.That(UserEntityCache.GetByName(user.Id, "New1").First().CategoryRelations.Count, Is.EqualTo(1));
        
        hasChildrenInUserCachedData = UserEntityCache.GetByName(user.Id, "A")
            .SelectMany(cCI => cCI.CachedData.ChildrenIds.Select(id => id)).ToList().IndexOf(newCat.Id) != -1;

        hasChildrenInEntityCachedData = EntityCache.GetByName("X2")
            .SelectMany(cCI => cCI.CachedData.ChildrenIds.Select(id => id)).ToList().IndexOf(newCat.Id) != -1; 

        Assert.That(hasChildrenInUserCachedData, Is.EqualTo(true));
        Assert.That(hasChildrenInEntityCachedData, Is.EqualTo(true));

    }

    [Test]
    public void Delete_category()
    {
        var cateContext = ContextCategory.New();
        var user1 = cateContext.AddCaseThreeToCache();
        var user = cateContext.AddCaseThreeToCache();

        EntityCache.Init();

        cateContext.Add("New", creator: user,parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();
        cateContext.Add("New1",creator: user, parent: Sl.CategoryRepo.GetByName("X2").First()).Persist();

        CategoryInKnowledge.Pin(EntityCache.GetByName("New").First().Id, user1);
        CategoryInKnowledge.Pin(EntityCache.GetByName("New1").First().Id, user1);

        var categoryNew = Sl.CategoryRepo.GetByName("New").First();

        var categoryCacheItem = EntityCache.GetByName("New").First();
        Resolve<CategoryDeleter>().Run(categoryNew);

        var userCaches  = UserEntityCache.GetAllCaches();
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

        categoryCacheItem = EntityCache.GetByName("New1").First();
    }

    private static bool HasRelation(CategoryCacheItem category, CategoryCacheRelation expectedRelation)
    {
       return category.CategoryRelations.Contains(expectedRelation); 
    }
}


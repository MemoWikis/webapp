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
       
       UserEntityCache.Init(true);
        var userEntityCacheCategories = UserEntityCache.GetCategories(user.Id).Values.ToList();
        var entityCacheCategories = EntityCache.GetAllCategories().ToList();

        // entityCacheCategories is uncut case and userEntityCacheCategoriess is cut case  https://app.diagrams.net/#G1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6

        //EntityCache
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "E").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "G").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("H").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("E").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("D").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "B").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("B").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X2").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X1").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X2").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X1").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X1").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X3").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));

        //userEntityCache
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "G").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("X").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("X3").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("B").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
    }

    [Test]
    public void Give_correct_number_of_cache_items()
    {
        ContextCategory.New().AddCaseThreeToCache();
        UserEntityCache.Init(true);
        var user = Sl.SessionUser.User;
        Assert.That(UserEntityCache.GetCategories(user.Id).Values.ToList().Count, Is.EqualTo(7));
        
        Sl.SessionUser.Logout();
        Assert.That(UserEntityCache.GetCategories(user.Id).Values.ToList().Count, Is.EqualTo(0));

        ContextCategory.New(false).AddCaseTwoToCache();
        Thread.Sleep(100);
        UserEntityCache.Init(true);
        user = Sl.SessionUser.User;
        Assert.That(UserEntityCache.GetCategories(user.Id).Values.ToList().Count, Is.EqualTo(5));
    }

    [Test]
    public void Give_correct_children()
    {
        ContextCategory.New().AddCaseThreeToCache();
        UserEntityCache.Init(true);
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
        ContextCategory.New().AddCaseThreeToCache();
        
        UserEntityCache.Init(true);

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
  

        Assert.That(nextParetFromX1.Name, Is.EqualTo("X3"));
        Assert.That(nextParetFromE.Name, Is.EqualTo("X"));
        Assert.That(nextParetFromD.Name, Is.EqualTo("B"));
        Assert.That(nextParetFromC.Name, Is.EqualTo("X"));
        Assert.That(nextParetFromH.Name, Is.EqualTo("X3"));
    }

    [Test]
    public void Test_init_for_all_user_entity_caches_change_name()
    {
        ContextCategory.New().AddCaseThreeToCache();

        var cate = EntityCache.GetAllCategories().First();
        cate.Name = "Daniel";
        UserEntityCache.ChangeAllActiveCategoryCaches(true);

        

        Assert.That(UserEntityCache.GetCategories(2).First().Value.Name, Is.EqualTo("Daniel"));
        Assert.That(UserEntityCache.GetCategories(2).Count, Is.EqualTo(7));


        var user = ContextUser.New().Add("Daniel").Persist().All.First();
        Sl.SessionUser.Login(user);
        var userEntityCacheAfterDeleteForUser3 = UserEntityCache.GetCategories(3);
        Assert.That(userEntityCacheAfterDeleteForUser3.Count, Is.EqualTo(1));
        Assert.That(UserEntityCache.GetCategories(3).First().Value.Name, Is.EqualTo("Daniel"));
    }

    [Test]
    public void Test_delete_category()
    {
        ContextCategory.New().AddCaseThreeToCache();

        CategoryInKnowledge.Pin(EntityCache.GetAllCategories().ByName("E").Id, Sl.UserRepo.GetById(2));
        Sl.CategoryRepo.Delete(EntityCache.GetAllCategories().ByName("E"));

        var userEntityCacheAfterDeleteForUser2 = UserEntityCache.GetCategories(2).Select(x => x.Value);

        Assert.That(userEntityCacheAfterDeleteForUser2.Count, Is.EqualTo(7));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("X").CategoryRelations.First().RelatedCategory.Name, Is.EqualTo("A"));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("X3").CategoryRelations.First().RelatedCategory.Name, Is.EqualTo("A"));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("B").CategoryRelations.First().RelatedCategory.Name, Is.EqualTo("A"));

        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count(), Is.EqualTo(0));

        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count(), Is.EqualTo(0));

        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "G").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count(), Is.EqualTo(1));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count(), Is.EqualTo(0));
        Assert.That(userEntityCacheAfterDeleteForUser2.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "E").Count(), Is.EqualTo(0));

        var t = UserEntityCache._Categories[UserEntityCache.CategoriesCacheKey(2)];
        
    
    }
}


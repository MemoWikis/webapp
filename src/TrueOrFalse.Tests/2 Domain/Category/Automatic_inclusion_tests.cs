using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TrueOrFalse.Tests;


class Automatic_inclusion_tests : BaseTest
{
    [Test]
    public void Test_Subcategory_add_correct_to_parent()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = ContextCategory
            .New()
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;

        context.Add(subCategories.ByName("Sub1").Name, subCategories[0].Type, parent: subCategories.ByName("Sub3"));
        EntityCache.Init();
        GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbUpdate( EntityCache.GetByName("Sub1").First());

        Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub1").Id).ParentCategories().Count, Is.EqualTo(2));
        Assert.That(Sl.CategoryRepo.GetById(parentA.Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(3));
        Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub3").Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(1));
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

    }

    [Test]
    public void Test_automatic_inclusion_user_entity_cache()
    {
        var user = ContextUser.New().Add("Dandor").Persist().All.First();
        Sl.SessionUser.Login(user);
        UserCache.GetItem(user.Id).IsFiltered = true;

        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = ContextCategory
            .New()
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;

        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub1").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub2").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub3").First().Id, user);

        context.Add(subCategories.ByName("Sub1").Name, subCategories[0].Type, parent: subCategories.ByName("Sub3"));
        EntityCache.Init();

        UserEntityCache.Init(user.Id);

        UserCache.GetItem(user.Id).IsFiltered = true;
        context.Add("SubSub1", parent: subCategories.ByName("Sub1"))
            .Persist();

        Assert.That(ContextCategory.HasCorrectIncludetContent(EntityCache.GetByName("Category").First(), "SubSub1", user.Id), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectIncludetContent(UserEntityCache.GetAllCategories(user.Id).ByName("Sub1"), "SubSub1", user.Id), Is.EqualTo(true));
    }

    [Test]
    public void Test_delete_user_cache_item()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = context
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;

        context.Add(subCategories.ByName("Sub1").Name, subCategories[0].Type, parent: subCategories.ByName("Sub3"));
        EntityCache.Init();


        var user = ContextUser.New().Add("Dandor").Persist().All.First();
        Sl.SessionUser.Login(user);

        context
            .Add("SubSub1", parent: subCategories.ByName("Sub1"))
            .Persist();

        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub1").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub2").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub3").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("SubSub1").First().Id, user);

        UserCache.GetItem(user.Id).IsFiltered = true;

        var idFromDeleteCategory = EntityCache.GetByName("SubSub1").First().Id; 

        context.Delete(Sl.CategoryRepo.GetByName("SubSub1").First());

        Assert.That(ContextCategory.isIdAvailableInRelations(UserEntityCache.GetAllCategories(user.Id).ByName("Category"), idFromDeleteCategory), Is.EqualTo(false));
        Assert.That(ContextCategory.isIdAvailableInRelations(UserEntityCache.GetAllCategories(user.Id).ByName("Sub1"),idFromDeleteCategory), Is.EqualTo(false));
    }

    [Test]
    public void Test_automatic_inclusion_user_entity_cache_by_update()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = ContextCategory
            .New()
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;
        
        ContextCategory
            .New()
            .Add("SubSub1", parent: subCategories.ByName("Sub1"))
            .Persist();

        context.Add(subCategories.ByName("Sub1").Name, subCategories[0].Type, parent: subCategories.ByName("Sub3")).Persist();
        EntityCache.Init();

      
        var user = ContextUser.New().Add("Dandor").Persist().All.First();
        Sl.SessionUser.Login(user);
      

        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub1").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub2").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub3").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("SubSub1").First().Id, user);

        var subSub1 = Sl.CategoryRepo.GetByName("SubSub1").First(); 
        subSub1.CategoryRelations.RemoveAt(0);
        subSub1.CategoryRelations.Add(new CategoryRelation{Category = subSub1, CategoryRelationType = CategoryRelationType.IsChildCategoryOf, RelatedCategory = Sl.CategoryRepo.GetByName("Sub2").First()});
        UserCache.GetItem(user.Id).IsFiltered = true;
        context.Update(subSub1); 
        
        Assert.That(ContextCategory.HasCorrectIncludetContent(UserEntityCache.GetAllCategories(user.Id).ByName("Category"), "SubSub1", user.Id), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectIncludetContent(UserEntityCache.GetAllCategories(user.Id).ByName("Sub2"), "SubSub1", user.Id), Is.EqualTo(true));
    }

    [Test]
    public void Test_Cached_Data()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = context
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;

        EntityCache.Init();
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

        Sl.CategoryRepo.Delete(context.All.ByName("Sub1"));
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));

        context.Add("Sub1", parent: parentA).Persist();
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

        context.All.ByName("Sub2").CategoryRelations.Add(new CategoryRelation
        {
            Category = context.All.ByName("Sub2"),
            CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
            RelatedCategory = context.All.ByName("Sub3")
        });
        Sl.CategoryRepo.Update(context.All.ByName("Sub2"));
        Assert.That(EntityCache.GetByName("Sub3").First().CachedData.ChildrenIds.Count, Is.EqualTo(1));


        context.All.ByName("Sub3").CategoryRelations.RemoveAt(0);
        Sl.CategoryRepo.Update(context.All.ByName("Sub3"));
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));

        context.All.ByName("Sub3").CategoryRelations.Add(new CategoryRelation
        {
            Category = context.All.ByName("Sub3"),
            CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
            RelatedCategory = Sl.CategoryRepo.GetByName("Sub1").First() 
        });
        Sl.CategoryRepo.Update(context.All.ByName("Sub3"));

        Assert.That(EntityCache.GetByName("Sub1").First().CachedData.ChildrenIds.Count, Is.EqualTo(1));
    }

    [Test]
   public void  Test_cached_date_user_entity_cache()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = context
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;
        

        EntityCache.Init();
        var user = ContextUser.New().Add("Dandor").Persist().All[0]; 
        Sl.SessionUser.Login(user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub1").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub2").First().Id, user);
        CategoryInKnowledge.Pin(EntityCache.GetByName("Sub3").First().Id, user);

        UserCache.GetItem(user.Id).IsFiltered = true;

        Sl.CategoryRepo.Delete(context.All.ByName("Sub1"));
        Assert.That(UserEntityCache.GetCategory(user.Id, parentA.Id).CachedData.ChildrenIds.Count, Is.EqualTo(2));
        context.Add("Sub1", parent: parentA).Persist();

        Assert.That(UserEntityCache.GetCategory(user.Id, EntityCache.GetByName("Category").First().Id).CachedData.ChildrenIds.Count, Is.EqualTo(3));

        context.All.ByName("Sub3").CategoryRelations.RemoveAt(0);
        Sl.CategoryRepo.Update(context.All.ByName("Sub3"));
        Assert.That(UserEntityCache.GetCategory(user.Id, EntityCache.GetByName("Category").First().Id).CachedData.ChildrenIds.Count, Is.EqualTo(2));

    }
}


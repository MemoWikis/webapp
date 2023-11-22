using System.Linq;
using Autofac;
using Autofac.Core.Lifetime;
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
            Resolve<EntityCacheInitializer>().Init();
            GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(
                EntityCache.GetCategoryByName("Sub1").First(), Resolve<SessionUser>().UserId);

            Assert.That(LifetimeScope.Resolve<CategoryRepository>().GetById(subCategories.ByName("Sub1").Id).ParentCategories().Count,
                Is.EqualTo(2));
            Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));
       
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
        LifetimeScope.Resolve<EntityCacheInitializer>().Init();


        var user = ContextUser.New(R<UserWritingRepo>()).Add("Dandor").Persist().All.First();
        LifetimeScope.Resolve<SessionUser>().Login(user);

        context
            .Add("SubSub1", parent: subCategories.ByName("Sub1"))
            .Persist();
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

        Resolve<EntityCacheInitializer>().Init();
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));
         
        EntityCache.Remove(context.All.ByName("Sub1").Id,Resolve<PermissionCheck>(), Resolve<SessionUser>().UserId);
        LifetimeScope.Resolve<CategoryRepository>().Delete(context.All.ByName("Sub1"));
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));

        context.Add("Sub1", parent: parentA).Persist();
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

        var categoryRelationToAdd = new CategoryRelation
        {
            Category = context.All.ByName("Sub2"),
            RelatedCategory = context.All.ByName("Sub3")
        };
        context.All.ByName("Sub2").CategoryRelations.Add(categoryRelationToAdd);
        var categoryCacheItem = EntityCache.GetCategoryByName("Sub2").FirstOrDefault();
        categoryCacheItem.CategoryRelations.Add(CategoryCacheRelation.ToCategoryCacheRelation(categoryRelationToAdd));
        LifetimeScope.Resolve<CategoryRepository>().Update(context.All.ByName("Sub2"));
        EntityCache.AddOrUpdate(categoryCacheItem);
        Assert.That(EntityCache.GetCategoryByName("Sub3").First().CachedData.ChildrenIds.Count, Is.EqualTo(1));


        context.All.ByName("Sub3").CategoryRelations.RemoveAt(0);
        categoryCacheItem = EntityCache.GetCategoryByName("Sub3").FirstOrDefault();
        categoryCacheItem.CategoryRelations.RemoveAt(0);
        LifetimeScope.Resolve<CategoryRepository>().Update(context.All.ByName("Sub3"));
        EntityCache.AddOrUpdate(categoryCacheItem);
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));

        categoryRelationToAdd = new CategoryRelation
        {
            Category = context.All.ByName("Sub3"),
            RelatedCategory = LifetimeScope.Resolve<CategoryRepository>().GetByName("Sub1").First()
        };

        context.All.ByName("Sub3").CategoryRelations.Add(categoryRelationToAdd);
        categoryCacheItem = EntityCache.GetCategoryByName("Sub3").FirstOrDefault();
        categoryCacheItem.CategoryRelations.Add(CategoryCacheRelation.ToCategoryCacheRelation(categoryRelationToAdd));
        LifetimeScope.Resolve<CategoryRepository>().Update(context.All.ByName("Sub3"));
        EntityCache.AddOrUpdate(categoryCacheItem);

        Assert.That(EntityCache.GetCategoryByName("Sub1").First().CachedData.ChildrenIds.Count, Is.EqualTo(1));
    }
}


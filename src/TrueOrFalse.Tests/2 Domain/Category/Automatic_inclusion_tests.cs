﻿using System.Collections.Generic;
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
        GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(EntityCache.GetCategoryByName("Sub1").First());

        Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub1").Id).ParentCategories().Count, Is.EqualTo(2));
        Assert.That(Sl.CategoryRepo.GetById(parentA.Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(3));
        Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub3").Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(1));
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

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
        SessionUser.Login(user);

        var subSub1 = Sl.CategoryRepo.GetByName("SubSub1").First();
        subSub1.CategoryRelations.RemoveAt(0);
        subSub1.CategoryRelations.Add(new CategoryRelation { Category = subSub1, CategoryRelationType = CategoryRelationType.IsChildOf, RelatedCategory = Sl.CategoryRepo.GetByName("Sub2").First() });
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
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

        EntityCache.Remove(context.All.ByName("Sub1").Id);
        Sl.CategoryRepo.Delete(context.All.ByName("Sub1"));
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));

        context.Add("Sub1", parent: parentA).Persist();
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

        var categoryRelationToAdd = new CategoryRelation
        {
            Category = context.All.ByName("Sub2"),
            CategoryRelationType = CategoryRelationType.IsChildOf,
            RelatedCategory = context.All.ByName("Sub3")
        };
        context.All.ByName("Sub2").CategoryRelations.Add(categoryRelationToAdd);
        var categoryCacheItem = EntityCache.GetCategoryByName("Sub2").FirstOrDefault();
        categoryCacheItem.CategoryRelations.Add(CategoryCacheRelation.ToCategoryCacheRelation(categoryRelationToAdd));
        Sl.CategoryRepo.Update(context.All.ByName("Sub2"));
        EntityCache.AddOrUpdate(categoryCacheItem);
        Assert.That(EntityCache.GetCategoryByName("Sub3").First().CachedData.ChildrenIds.Count, Is.EqualTo(1));


        context.All.ByName("Sub3").CategoryRelations.RemoveAt(0);
        categoryCacheItem = EntityCache.GetCategoryByName("Sub3").FirstOrDefault();
        categoryCacheItem.CategoryRelations.RemoveAt(0);
        Sl.CategoryRepo.Update(context.All.ByName("Sub3"));
        EntityCache.AddOrUpdate(categoryCacheItem);
        Assert.That(EntityCache.GetCategoryByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));

        categoryRelationToAdd = new CategoryRelation
        {
            Category = context.All.ByName("Sub3"),
            CategoryRelationType = CategoryRelationType.IsChildOf,
            RelatedCategory = Sl.CategoryRepo.GetByName("Sub1").First()
        };

        context.All.ByName("Sub3").CategoryRelations.Add(categoryRelationToAdd);
        categoryCacheItem = EntityCache.GetCategoryByName("Sub3").FirstOrDefault();
        categoryCacheItem.CategoryRelations.Add(CategoryCacheRelation.ToCategoryCacheRelation(categoryRelationToAdd));
        Sl.CategoryRepo.Update(context.All.ByName("Sub3"));
        EntityCache.AddOrUpdate(categoryCacheItem);

        Assert.That(EntityCache.GetCategoryByName("Sub1").First().CachedData.ChildrenIds.Count, Is.EqualTo(1));
    }
}


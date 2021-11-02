using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Diagnostics;
using GraphJsonDtos;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Crumbtrail_test : BaseTest
{
    [Test]
    public void Should_get_crumbtrail()
    {
        var context = ContextCategory.New();
        var rootElement =  context.Add("1").Persist().All.First();

        var firstLevelChildren = context
            .Add("1.1", parent: rootElement)
            .Add("1.2", parent: rootElement)
            .Persist()
            .All;

        var secondLevelChildren = context
            .Add("1.1.1", parent: firstLevelChildren.ByName("1.1"))
            .Add("1.2.1", parent: firstLevelChildren.ByName("1.2"))
            .Add("1.2.2", parent: firstLevelChildren.ByName("1.2"))
            .Persist()
            .All;
        
        var thirdLevelChildren = context
            .Add("1.1.1.1", parent: secondLevelChildren.ByName("1.1.1"))
            .Add("1.2.1.1", parent: secondLevelChildren.ByName("1.2.1"))
            .Add("1.2.1.2", parent: secondLevelChildren.ByName("1.2.1"))
            .Add("1.2.2.1", parent: secondLevelChildren.ByName("1.2.2"))
            .Persist() 
            .All;


        var rootElementAsCache = CategoryCacheItem.ToCacheCategory(rootElement);
        
        var crumbtrail_1_1_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.1.1.1")), rootElementAsCache);
        Assert.That(crumbtrail_1_1_1_1.ToDebugString(), Is.EqualTo("1 => 1.1 => 1.1.1 => [1.1.1.1]"));

        var crumbtrail_1_2_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.2.1.1")), rootElementAsCache);
        Assert.That(crumbtrail_1_2_1_1.ToDebugString(), Is.EqualTo("1 => 1.2 => 1.2.1 => [1.2.1.1]"));

        var crumbtrail_1_2_1_2 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.2.1.2")), rootElementAsCache);
        Assert.That(crumbtrail_1_2_1_2.ToDebugString(), Is.EqualTo("1 => 1.2 => 1.2.1 => [1.2.1.2]"));

        var crumbtrail_1_2_2_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.2.2.1")), rootElementAsCache);
        Assert.That(crumbtrail_1_2_2_1.ToDebugString(), Is.EqualTo("1 => 1.2 => 1.2.2 => [1.2.2.1]"));


        //add circular references
        context
            .Add("1.2", parent: secondLevelChildren.ByName("1.2.1"))
            .Persist();

        var crumbtrail_1_2_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.2.1")), rootElementAsCache);
        Assert.That(crumbtrail_1_2_1.ToDebugString(), Is.EqualTo("1 => 1.2 => [1.2.1]"));
    }

    [Test]
    public void Should_get_correct_RootWiki()
    {
        var context = ContextCategory.New();
        var firstRoot = context.Add("first").Persist().All.First();
        var secondRoot = context.Add("second").Persist().All.First();
        var rootOfFirstRoot = context.Add("rofr").Persist().All.First();
        var globalRoot = context.Add("global").Persist().All.First();
        var branchRoot = context.Add("branch").Persist().All.First();


        var userContext = ContextUser.New();
        var branchUser = userContext.Add("secondUser").Persist().All.First();

        var rootOfFirstRootContext = context
            .Add("first", parent: rootOfFirstRoot)
            .Persist().All;
        
        var firstLevelChildren = context
            .Add("1.1", parent: firstRoot)
            .Add("1.2", parent: firstRoot)
            .Add("g.1", parent: globalRoot)
            .Persist()
            .All;

        var secondLevelChildren = context
            .Add("1.1.1", parent: firstLevelChildren.ByName("1.1"))
            .Add("1.2.1", parent: firstLevelChildren.ByName("1.2"))
            .Add("g.1.1", parent: firstLevelChildren.ByName("g.1"))
            .Add("g.1.1", parent: secondRoot)
            .Add("x.1.1", parent: secondRoot)
            .Persist()
            .All;

        var thirdLevelChildren = context
            .Add("1.1.1.1", parent: secondLevelChildren.ByName("1.1.1"))
            .Add("1.2.1.1", parent: secondLevelChildren.ByName("1.2.1"))
            .Add("g.1.1.1", parent: secondLevelChildren.ByName("g.1.1"))
            .Add("x.1.1.1", parent: secondLevelChildren.ByName("x.1.1"))
            .Add("xg.1.1.1", parent: secondLevelChildren.ByName("g.1.1"))
            .Persist()
            .All;

        var firstRootAsCache = CategoryCacheItem.ToCacheCategory(firstRoot);
        var secondRootAsCache = CategoryCacheItem.ToCacheCategory(secondRoot);
        var rootOfFirstRootAsCache = CategoryCacheItem.ToCacheCategory(rootOfFirstRoot);
        var globalRootAsCache = CategoryCacheItem.ToCacheCategory(globalRoot);


        var crumbtrail_1_2_1_1 = CrumbtrailService.BuildCrumbtrail(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.2.1.1")), firstRootAsCache);
        Assert.That(crumbtrail_1_2_1_1.ToDebugString(), Is.EqualTo("first => 1.2 => 1.2.1 => [1.2.1.1]"));

        var crumbtrail_x_1_1_1 = CrumbtrailService.BuildCrumbtrail(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("x.1.1.1")), secondRootAsCache);
        Assert.That(crumbtrail_x_1_1_1.ToDebugString(), Is.EqualTo("second => x.1.1 => [x.1.1.1]"));

        var crumbtrail_xg_1_1_1 = CrumbtrailService.BuildCrumbtrail(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("xg.1.1.1")), secondRootAsCache);
        Assert.That(crumbtrail_xg_1_1_1.ToDebugString(), Is.EqualTo("second => g.1.1 => [xg.1.1.1]"));

        var crumbtrail_1_1_1_1 = CrumbtrailService.BuildCrumbtrail(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.1.1.1")), firstRootAsCache);
        Assert.That(crumbtrail_1_1_1_1.ToDebugString(), Is.EqualTo("first => 1.1 => 1.1.1 => [1.1.1.1]"));

        var crumbtrail_rofr_1_1_1_1 = CrumbtrailService.BuildCrumbtrail(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.1.1.1")), rootOfFirstRootAsCache);
        Assert.That(crumbtrail_rofr_1_1_1_1.ToDebugString(), Is.EqualTo("rofr => first => 1.1 => 1.1.1 => [1.1.1.1]"));
    }

    [Test]
    public void Get_Correct_BreadcrumbItems()
    {
        var categoryContext = ContextCategory.New();
        var userContext = ContextUser.New();

        var aWikiUser = userContext.Add("aWikiUser").Persist().All.First();
        var aWiki = categoryContext.Add("aWiki", creator: aWikiUser).Persist().All.First();

        var bWikiUser = userContext.Add("bWikiUser").Persist().All.First();
        var bWiki = categoryContext.Add("bWiki", creator: bWikiUser).Persist().All.First();
        var bWikiCache = CategoryCacheItem.ToCacheCategory(bWiki);

        var cWikiUser = userContext.Add("cWikiUser").Persist().All.First();
        var cWiki = categoryContext.Add("cWiki", creator: cWikiUser).Persist().All.First();
        var cWikiCache = CategoryCacheItem.ToCacheCategory(cWiki);

        var memuchoWikiUser = userContext.Add("memuchoWikiUser").Persist().All.First();
        var memuchoWiki = categoryContext.Add("memuchoWiki", creator: memuchoWikiUser).Persist().All.First();
        var memuchoWikiCache = CategoryCacheItem.ToCacheCategory(memuchoWiki);

        var bPersonalx_0_0_0 = categoryContext.Add("bPersonalx_0_0_0", parent: bWiki, creator: bWikiUser).Persist().All.First();

        var memuchoCat1Parents = new List<Category>();
        memuchoCat1Parents.Add(memuchoWiki);
        memuchoCat1Parents.Add(aWiki);
        memuchoCat1Parents.Add(bPersonalx_0_0_0);
        var memuchoCat0_x_0_0 = categoryContext.Add("memuchoCat0_x_0_0", parents: memuchoCat1Parents, creator: memuchoWikiUser).Persist().All.First();

        var memuchoCat0_0_x_0 = categoryContext.Add("memuchoCat0_0_x_0", parent: memuchoCat0_x_0_0, creator: memuchoWikiUser).Persist().All.First();
        var aPersonal0_0_x_0 = categoryContext.Add("aPersonalCat0_0_x_0", parent: memuchoCat0_x_0_0, creator: aWikiUser).Persist().All.First();

        var currentCategoryParents = new List<Category>();
        currentCategoryParents.Add(memuchoCat0_0_x_0);
        currentCategoryParents.Add(aPersonal0_0_x_0);
        var currentCategory0_0_0_x = categoryContext.Add("memuchoCat0_0_0_x", parents: memuchoCat1Parents, creator: memuchoWikiUser).Persist().All.First();
    }

    [Test]
    public void Get_Correct_Wiki()
    {
        var contextCategory = ContextCategory.New();

        var filler1 = contextCategory.Add(categoryName: "category name 1", id: 1).Persist().All.ByName("category name 1");
        var filler2 = contextCategory.Add(categoryName: "category name 2", id: 2).Persist().All.ByName("category name 2");
        var filler3 = contextCategory.Add(categoryName: "category name 3", id: 3).Persist().All.ByName("category name 3");
        var filler4 = contextCategory.Add(categoryName: "category name 4", id: 4).Persist().All.ByName("category name 4");
        var category5 = contextCategory.Add(categoryName: "category name 5", id: 5).Persist().All.ByName("category name 5");
        var childOf5 = contextCategory.Add(categoryName: "child of 5", parent: category5, id: 6).Persist().All.ByName("child of 5");



        EntityCache.Init();

        var sessionUser = Sl.SessionUser;
        var beforeSettingId = sessionUser.CurrentWikiId;

        var filler3Cache = EntityCache.GetCategoryCacheItem(filler3.Id);
        var childOf5Cache = EntityCache.GetCategoryCacheItem(childOf5.Id);
        sessionUser.SetWikiId(filler3Cache);

        var wikiIdShouldBe3 = sessionUser.CurrentWikiId;

        Assert.That(beforeSettingId, Is.EqualTo(1));
        Assert.That(wikiIdShouldBe3, Is.EqualTo(3));

        var newWikiId = CrumbtrailService.GetWiki(childOf5Cache).Id;

        Assert.That(newWikiId, Is.EqualTo(5));
    }
}

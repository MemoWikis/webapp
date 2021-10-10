using System.Linq;
using FluentNHibernate.Conventions.Inspections;
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


        var crumbtrail_1_2_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.2.1.1")), firstRootAsCache);
        Assert.That(crumbtrail_1_2_1_1.ToDebugString(), Is.EqualTo("first => 1.2 => 1.2.1 => [1.2.1.1]"));

        var crumbtrail_x_1_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("x.1.1.1")), secondRootAsCache);
        Assert.That(crumbtrail_x_1_1_1.ToDebugString(), Is.EqualTo("second => x.1.1 => [x.1.1.1]"));

        var crumbtrail_xg_1_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("xg.1.1.1")), secondRootAsCache);
        Assert.That(crumbtrail_xg_1_1_1.ToDebugString(), Is.EqualTo("second => g.1.1 => [xg.1.1.1]"));

        var crumbtrail_1_1_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.1.1.1")), firstRootAsCache);
        Assert.That(crumbtrail_1_1_1_1.ToDebugString(), Is.EqualTo("first => 1.1 => 1.1.1 => [1.1.1.1]"));

        var crumbtrail_rofr_1_1_1_1 = CrumbtrailService.Get(CategoryCacheItem.ToCacheCategory(thirdLevelChildren.ByName("1.1.1.1")), rootOfFirstRootAsCache);
        Assert.That(crumbtrail_rofr_1_1_1_1.ToDebugString(), Is.EqualTo("rofr => first => 1.1 => 1.1.1 => [1.1.1.1]"));
    }
}

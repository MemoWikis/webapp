using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Tests;


class User_world_tests : BaseTest
{
    [Test]
    public void Should_return_correct_categories()
    {
        var context = ContextCategory.New();

        var rootElement = context.Add("A").Persist().All.First();

        var firstChildren = context
            .Add("X", parent: rootElement)
            .Add("X1", parent: rootElement)
            .Add("X2", parent: rootElement)
            .Add("X3", parent: rootElement)
            .Persist()
            .All;

        context
            .Add("X1", parent: firstChildren.ByName("X3"))
            .Persist();

        var secondChildren = context
            .Add("B", parent: rootElement)
            .Add("C", parent: firstChildren.ByName("X"))
            .Persist()
            .All;

        context
            .Add("C", parent: firstChildren.ByName("X1"))
            .Persist();

        context
            .Add("X1", parent: firstChildren.ByName("X2"))
            .Persist();

        var ThirdChildren = context
            .Add("H", parent: firstChildren.ByName("C"))
            .Add("G", parent: firstChildren.ByName("C"))
            .Add("F", parent: firstChildren.ByName("C"))
            .Add("E", parent: firstChildren.ByName("C"))
            .Add("D", parent: firstChildren.ByName("B"))
            .Persist()
            .All;

        context
            .Add("I", parent: secondChildren.ByName("C"))
            .Persist();

        context
            .Add("I", parent: secondChildren.ByName("E"))
            .Persist();

        context.Add("I", parent: secondChildren.ByName("G"))
            .Persist();


        var user = ContextUser.New().Add("User").Persist().All[0];

        // Add in WUWI
        CategoryInKnowledge.Pin(firstChildren.ByName("B").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("G").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("F").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("I").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("X").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("X3").Id, user);

        Sl.SessionUser.Login(user);
        var userWorldCacheCategories = UserEntityCache.GetCategories(user.Id);

        //userWorldCacheCategories
        //Assert.That(typeof(IDictionary).IsAssignableFrom( userWorldCacheCategories), Is.EqualTo(true));
        //var categoriesList = new List<Category>();
        //var t = userWorldCacheCategories; 
        
    }
}


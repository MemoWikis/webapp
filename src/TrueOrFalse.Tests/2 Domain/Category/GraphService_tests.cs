using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

class GraphService_tests : BaseTest
{
    [Test]
    public void Should_get_correct_category()
    {
        var context = ContextCategory.New();

        var rootElement = context.Add("RootElement").Persist().All.First();

        var firstChildrens = context
            .Add("Sub1", parent: rootElement)
            .Persist()
            .All;

        var secondChildren = context.
            Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");


        // Add User
        var user = ContextUser.New().Add("User").Persist().All[0];

        //Add thirdChildren and Sub2 as Wuwi
        
        CategoryInKnowledge.Pin(firstChildrens.ByName("SubSub1").Id, user);

        var t = GraphService.GetWuwiChildrenCategories(rootElement.Id);

        //Assert.That();

    }



    [Test]
    public void Add_should_category_history_second()
    {

        var test =
            @"
Arrange: 

A -> B -> +C
A(2) -> B(4) -> +C(5)


Act:
Filter nur Wunschwissen

Assert:
A -> C



";

        var context = ContextCategory.New();

        var rootElement = context.Add("RootElement").Persist().All.First();


        var firstChildrens = context
            .Add("Sub1", parent: rootElement)
            .Add("Sub2", parent: rootElement)
            .Persist()
            .All;

        var secondChildren = context.
            Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");

        var thirdChildren = context.Add("SubSubSub1", parent: secondChildren).Persist().All.ByName("SubSubSub1");

        // Add User
        var user = ContextUser.New().Add("User").Persist().All[0];

        //Add thirdChildren and Sub2 as Wuwi
        CategoryInKnowledge.Pin(thirdChildren.Id, user);
        CategoryInKnowledge.Pin(firstChildrens.ByName("Sub2").Id, user);


        Assert.That(GraphService
            .GetAllParents(Sl.CategoryRepo
                .GetByName("SubSubSub1")
                .First()
                .Id)
            .Where(c => c.IsInWishknowledge())
            .ToList()
            .Count, Is.EqualTo(0));


        Assert.That(
            GraphService
                .GetAllParents(Sl.CategoryRepo.GetByName("SubSubSub1")
                    .First()
                    .Id).Where(c => c.Name == "RootElement").ToList().Count
            ,
            Is.EqualTo(1));

    }
}


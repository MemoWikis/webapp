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


        CategoryInKnowledge.Pin(firstChildrens.ByName("SubSub1").Id, user);

        Sl.SessionUser.Login(user);
        var lastChildren = GraphService.GetLastWuwiChildrenFromCategories(rootElement.Id);

        Assert.That(lastChildren.First().Name, Is.EqualTo("SubSub1"));

    }



    [Test]
    public void Should_get_correct_category_with_relations()
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
            .Persist()
            .All;

        var secondChildren = context.
            Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");


        // Add User
        var user = ContextUser.New().Add("User").Persist().All[0];


        CategoryInKnowledge.Pin(firstChildrens.ByName("SubSub1").Id, user);

        Sl.SessionUser.Login(user);
      var userPersonelCategoriesWithRealtions =   GraphService.GetAllPersonelCategoriesWithRealtions(rootElement);

      Assert.That(userPersonelCategoriesWithRealtions.First().Name, Is.EqualTo("SubSub1"));
      Assert.That(userPersonelCategoriesWithRealtions.First().CategoryRelations.First().RelatedCategory.Name, Is.EqualTo("RootElement"));
      Assert.That(userPersonelCategoriesWithRealtions.First().CategoryRelations.First().Category.Name, Is.EqualTo("SubSub1"));
      Assert.That(userPersonelCategoriesWithRealtions.First().CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));

    }
    [Test]
    public void Has_no_name()
    {
        //https://docs.google.com/drawings/d/1Wbne-XXmYkA578uSc6nY0mxz_s-pG8E9Q9flmgY2ZNY/

        var context = ContextCategory.New();

        var rootElement = context.Add("A").Persist().All.First();

        var firstChildren = context
            .Add("B", parent: rootElement)
            .Add("C", parent:rootElement)
            .Persist()
            .All;

        var secondChildren = context
            .Add("H", parent: firstChildren.ByName("C"))
            .Add("G", parent: firstChildren.ByName("C"))
            .Add("F", parent: firstChildren.ByName("C"))
            .Add("E", parent: firstChildren.ByName("C"))
            .Add("I", parent: firstChildren.ByName("C"))
            .Add("D", parent: firstChildren.ByName("B"))
            .Persist()
            .All;

        // Add User
        var user = ContextUser.New().Add("User").Persist().All[0];

        CategoryInKnowledge.Pin(firstChildren.ByName("C").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("G").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("E").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("I").Id, user);

        Sl.SessionUser.Login(user);

        var userPersonelCategoriesWithRealtions = GraphService.GetAllPersonelCategoriesWithRealtions(rootElement);

        Assert.That(userPersonelCategoriesWithRealtions.ByName("C").CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));
        Assert.That(userPersonelCategoriesWithRealtions.ByName("C").CategoryRelations.First().RelatedCategory.Id, Is.EqualTo(rootElement.Id));
        Assert.That(userPersonelCategoriesWithRealtions.ByName("C").CategoryRelations.First().Category.Id, Is.EqualTo(secondChildren.ByName("C").Id));

    }
}


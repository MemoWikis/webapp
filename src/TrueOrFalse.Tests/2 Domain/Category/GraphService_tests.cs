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
    public void Wish_knowledge_filter_simple_test()
    {
        // Case https://docs.google.com/drawings/d/1Wbne-XXmYkA578uSc6nY0mxz_s-pG8E9Q9flmgY2ZNY/

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
        CategoryInKnowledge.Pin(firstChildren.ByName("C").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("G").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("E").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("I").Id, user);

        Sl.SessionUser.Login(user);

        var userPersonelCategoriesWithRealtions = GraphService.GetAllPersonelCategoriesWithRealtions(rootElement);

        //Test C
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions
            .ByName("C").CategoryRelations), 
            Is.EqualTo(true));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("C")
                .CategoryRelations
                .First()
                .RelatedCategory.Id,
                Is.EqualTo(rootElement.Id));

        Assert.That(userPersonelCategoriesWithRealtions
            .ByName("C").CategoryRelations
            .First()
            .Category.Id, 
            Is.EqualTo(secondChildren.ByName("C").Id));


        //Test I
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions
            .ByName("I").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations
            .First()
            .RelatedCategory.Id,
            Is.EqualTo(secondChildren.ByName("C").Id));

        Assert.That(userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations
            .First()
            .Category.Id, 
            Is.EqualTo(secondChildren.ByName("I").Id));

        var relationId = userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations.Where(cr => cr.RelatedCategory.Name == "E" ).Select(cr => cr.RelatedCategory.Id).First();
           
        Assert.That(relationId,
            Is.EqualTo(secondChildren.ByName("E").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("I")
                .CategoryRelations[1]
                .Category.Id,
            Is.EqualTo(secondChildren.ByName("I").Id));

        relationId = userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations.Where(cr => cr.RelatedCategory.Name == "G").Select(cr => cr.RelatedCategory.Id).First();

        Assert.That(relationId,
            Is.EqualTo(secondChildren.ByName("G").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("I")
                .CategoryRelations[2]
                .Category.Id,
            Is.EqualTo(secondChildren.ByName("I").Id));


        // Test G 
        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("G").CategoryRelations
                .First()
                .CategoryRelationType,
            Is.EqualTo(CategoryRelationType.IsChildCategoryOf));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("G")
                .CategoryRelations
                .First()
                .RelatedCategory.Id,
            Is.EqualTo(firstChildren.ByName("C").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("G").CategoryRelations
                .First()
                .Category.Id,
            Is.EqualTo(secondChildren.ByName("G").Id));

        // Test E

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("E").CategoryRelations
                .First()
                .CategoryRelationType,
            Is.EqualTo(CategoryRelationType.IsChildCategoryOf));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("E")
                .CategoryRelations
                .First()
                .RelatedCategory.Id,
            Is.EqualTo(firstChildren.ByName("C").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("E").CategoryRelations
                .First()
                .Category.Id,
            Is.EqualTo(secondChildren.ByName("E").Id));

    }

    [Test]
    public void Wish_knowledge_filter_middle_test()
    {
        var context = ContextCategory.New();

        var rootElement = context.Add("A").Persist().All.First();

        var firstChildren = context
            .Add("B", parent: rootElement)
            .Add("C", parent: rootElement)
            .Persist()
            .All;

        var secondChildren = context
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
        CategoryInKnowledge.Pin(firstChildren.ByName("E").Id, user);
        CategoryInKnowledge.Pin(firstChildren.ByName("I").Id, user);

        Sl.SessionUser.Login(user);

        var userPersonelCategoriesWithRealtions = GraphService.GetAllPersonelCategoriesWithRealtions(rootElement);

        //Test I
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions
           .ByName("I").CategoryRelations)
           , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions
            .ByName("I")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions
                .ByName("I"), "A"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions
                .ByName("I"), "E" ),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions
                .ByName("I"), "G"),
            Is.EqualTo(true));

        //Test B
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions
                .ByName("B").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions
                .ByName("B")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions
                .ByName("B"), "A"),
            Is.EqualTo(true));

        //Test E
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions
                .ByName("E").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions
                .ByName("E")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions
                .ByName("E"), "A"),
            Is.EqualTo(true));

        //Test G
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions
                .ByName("G").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions
                .ByName("G")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions
                .ByName("G"), "A"),
            Is.EqualTo(true));
    }


    private bool IsAllRelationsAChildOf(IList<CategoryRelation> categoryRelations)
    {
        var result = true;
        foreach (var cr in categoryRelations)
        {
            if (cr.CategoryRelationType != CategoryRelationType.IsChildCategoryOf)
                result = false;
        }

        return result; 
    }

    private bool HasCorrectParent(Category category, string nameParent)
    {
        return category.CategoryRelations.Any(cr => cr.RelatedCategory.Name == nameParent);
    }

    private bool IsCategoryRelationsCategoriesIdCorrect(Category category)
    {
        return category.CategoryRelations.Select(cr => cr.Category.Name == category.Name).All(b => b == true);
    }
}


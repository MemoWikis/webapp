using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;
using TrueOrFalse.Tests;

class GraphService_tests : BaseTest
{
    [Test]
    public void Should_get_correct_category_with_relations()
    {

        var context = ContextCategory.New();

        var rootElement = context.Add("RootElement").Persist().All.First();

        var firstChildrenIdss = context
            .Add("Sub1", parent: rootElement)
            .Persist()
            .All;

        var secondChildrenIds = context.
            Add("SubSub1", parent: firstChildrenIdss.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");


        // Add User
        var user = ContextUser.New().Add("User").Persist().All[0];


        CategoryInKnowledge.Pin(firstChildrenIdss.ByName("SubSub1").Id, user);

        Sl.SessionUser.Login(user);
        EntityCache.Clear();
        EntityCache.Init();

        var userPersonnelCategoriesWithRelations =   GraphService.GetAllPersonalCategoriesWithRelations_TP(CategoryCacheItem.ToCacheCategory(rootElement), 2);

        Assert.That(userPersonnelCategoriesWithRelations.ByName("SubSub1").Name, Is.EqualTo("SubSub1"));
        Assert.That(EntityCache.GetCategoryCacheItem(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations.First().RelatedCategoryId).Name, Is.EqualTo("RootElement"));
        Assert.That(EntityCache.GetCategoryCacheItem(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations.First().CategoryId).Name, Is.EqualTo("SubSub1"));
        Assert.That(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations.First().CategoryRelationType, Is.EqualTo(CategoryRelationType.IsChildCategoryOf));

    }

    [Test]
    public void Wish_knowledge_filter_simple_test()
    {
        // Case https://docs.google.com/drawings/d/1Wbne-XXmYkA578uSc6nY0mxz_s-pG8E9Q9flmgY2ZNY/

        var context = ContextCategory.New();

        var rootElement = context.Add("A").Persist().All.First();

        var firstChildrenIds = context
            .Add("B", parent: rootElement)
            .Add("C", parent:rootElement)
            .Persist()
            .All;

        var secondChildrenIds = context
            .Add("H", parent: firstChildrenIds.ByName("C"))
            .Add("G", parent: firstChildrenIds.ByName("C"))
            .Add("F", parent: firstChildrenIds.ByName("C"))
            .Add("E", parent: firstChildrenIds.ByName("C"))
            .Add("D", parent: firstChildrenIds.ByName("B"))
            .Persist()
            .All;

        context
            .Add("I", parent: secondChildrenIds.ByName("C"))
            .Persist();

        context
            .Add("I", parent: secondChildrenIds.ByName("E"))
            .Persist();

        context.Add("I", parent: secondChildrenIds.ByName("G"))
            .Persist();


        var user = ContextUser.New().Add("User").Persist().All[0];

        // Add in WUWI
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("C").Id, user);
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("G").Id, user);
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("E").Id, user);
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("I").Id, user);

        Sl.SessionUser.Login(user);
        EntityCache.Init();

        var userPersonelCategoriesWithRealtions = GraphService.GetAllPersonalCategoriesWithRelations_TP(CategoryCacheItem.ToCacheCategory(rootElement),user.Id);
         
        //Test C
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions.ByName("C").CategoryRelations), 
            Is.EqualTo(true));

        Assert.That(userPersonelCategoriesWithRealtions.ByName("C").CategoryRelations.First().RelatedCategoryId, 
            Is.EqualTo(rootElement.Id));

        Assert.That(userPersonelCategoriesWithRealtions
            .ByName("C").CategoryRelations
            .First()
            .CategoryId, 
            Is.EqualTo(secondChildrenIds.ByName("C").Id));

        //Test I
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions.ByName("I").CategoryRelations), 
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("I"), "C") ,
            Is.EqualTo(true));

        Assert.That(userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations
            .First()
            .CategoryId, 
            Is.EqualTo(secondChildrenIds.ByName("I").Id));

        var relationId = userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "E" ).Select(cr => cr.RelatedCategoryId).First();
           
        Assert.That(relationId,
            Is.EqualTo(secondChildrenIds.ByName("E").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("I")
                .CategoryRelations[1]
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("I").Id));

        relationId = userPersonelCategoriesWithRealtions
            .ByName("I")
            .CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "G").Select(cr => cr.RelatedCategoryId).First();

        Assert.That(relationId,
            Is.EqualTo(secondChildrenIds.ByName("G").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("I")
                .CategoryRelations[2]
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("I").Id));


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
                .RelatedCategoryId,
            Is.EqualTo(firstChildrenIds.ByName("C").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("G").CategoryRelations
                .First()
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("G").Id));

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
                .RelatedCategoryId,
            Is.EqualTo(firstChildrenIds.ByName("C").Id));

        Assert.That(userPersonelCategoriesWithRealtions
                .ByName("E").CategoryRelations
                .First()
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("E").Id));

    }

    [Test]
    public void Wish_knowledge_filter_middle_test()
    {
        ContextCategory.New().AddCaseTwoToCache();
        EntityCache.Init();
        var rootElement = EntityCache.GetAllCategories().First();

        var userPersonelCategoriesWithRealtions = GraphService.GetAllPersonalCategoriesWithRelations_TP(rootElement,2);

        //Test I
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions.ByName("I").CategoryRelations)
           , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions.ByName("I")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("I"), "A"),
            Is.EqualTo(false));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("I"), "E" ),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("I"), "G"),
            Is.EqualTo(true));

        //Test B
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions.ByName("B").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions.ByName("B")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("B"), "A"),
            Is.EqualTo(true));

        //Test E
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions.ByName("E").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions.ByName("E")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("E"), "A"),
            Is.EqualTo(true));

        //Test G
        Assert.That(IsAllRelationsAChildOf(userPersonelCategoriesWithRealtions.ByName("G").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonelCategoriesWithRealtions.ByName("G")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonelCategoriesWithRealtions.ByName("G"), "A"),
            Is.EqualTo(true));
    }
    [Test]
    public void Wish_knowledge_filter_complex_test()
    {
        ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        var rootElement = EntityCache.GetAllCategories().ByName("A"); 

        var allPersonalCategoriesWithRelations = GraphService.GetAllPersonalCategoriesWithRelations_TP(rootElement, 2);

        //Test I
        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("I").CategoryRelations), 
            Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("I")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("I"), "A"),
            Is.EqualTo(false));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("I"), "G"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("I"), "X3"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("I"), "X"),
            Is.EqualTo(true));


        //Test G
        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("G").CategoryRelations), 
            Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("G")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("G"), "X3"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("G"), "X"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("G"), "A"),
            Is.EqualTo(false));


        //Test F
        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("F").CategoryRelations), 
            Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("F")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("F"), "X3"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("F"), "X"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("F"), "A"),
            Is.EqualTo(false));

        //Test X3
        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("X3").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("X3")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("X3"), "A"),
            Is.EqualTo(true));

        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("X").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("X")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("X"), "A"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("X"), "I"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("X"), "G"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("X"), "F"),
            Is.EqualTo(true));

        Assert.That(allPersonalCategoriesWithRelations.ByName("X").CachedData.ChildrenIds.Count,
            Is.EqualTo(3));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("X3"), "F"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("X3"), "G"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("X3"), "I"), 
            Is.EqualTo(true));

        Assert.That(allPersonalCategoriesWithRelations.ByName("X").CachedData.ChildrenIds.Count,
            Is.EqualTo(3));
     
        Assert.That(allPersonalCategoriesWithRelations.ByName("B").CachedData.ChildrenIds.Count,
            Is.EqualTo(0));

        Assert.That(allPersonalCategoriesWithRelations.ByName("F").CachedData.ChildrenIds.Count,
            Is.EqualTo(0));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("G"), "I"),
            Is.EqualTo(true));

        Assert.That(allPersonalCategoriesWithRelations.ByName("G").CachedData.ChildrenIds.Count,
            Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("A"), "B"),
            Is.EqualTo(true)); 

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("A"), "X"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName("A"), "X3"), 
            Is.EqualTo(true));

        Assert.That(allPersonalCategoriesWithRelations.ByName("A").CachedData.ChildrenIds.Count,
            Is.EqualTo(3));
    }

    [Test]
    public void Wish_knowledge_filter_special_case()
    {
        https://docs.google.com/drawings/d/1CWJoFSk5aAJf1EOpWqf1Ffr6ncjwxpOcJqFWZEXUVk4

        var context = ContextCategory.New();

        var rootElement = context.Add("A").Persist().All.First();

        var firstChildrenIds = context
            .Add("B", parent: rootElement)
            .Add("C", parent: rootElement)
            .Add("D", parent: rootElement)
            .Persist()
            .All;

        context
            .Add("D", parent: firstChildrenIds.ByName("C"))
            .Persist();

        var secondChildrenIds = context
            .Add("E", parent: firstChildrenIds.ByName("C"))
            .Persist()
            .All;

        var ThirdChild = context
            .Add("F", parent: firstChildrenIds.ByName("E"))
            .Persist()
            .All;

        var user = ContextUser.New().Add("User").Persist().All[0];

        // Add in WUWI
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("F").Id, user);
        
        Sl.SessionUser.Login(user);
        EntityCache.Init();
        UserEntityCache.Clear();

        var userPersonnelCategoriesWithRelations = GraphService.GetAllPersonalCategoriesWithRelations_TP(CategoryCacheItem.ToCacheCategory(rootElement), 2);

        //Test F
        Assert.That(IsAllRelationsAChildOf(userPersonnelCategoriesWithRelations.ByName("F").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonnelCategoriesWithRelations
                .ByName("F")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonnelCategoriesWithRelations.ByName("F"), "A"),
            Is.EqualTo(true));
        Assert.That(userPersonnelCategoriesWithRelations.ByName("F").CategoryRelations.Count, Is.EqualTo(1));

    }

    [Test]
    public void Without_wish_knowledge()
    {
        RecycleContainer();
        ContextCategory.New().AddCaseThreeToCache(false);
        EntityCache.Init();
        
        var rootElement = EntityCache.GetAllCategories().ByName("A");

        var userPersonelCategoriesWithRealtions = GraphService.GetAllPersonalCategoriesWithRelations_TP(rootElement, 2);
        Assert.That(userPersonelCategoriesWithRealtions.Count, Is.EqualTo(1)); //root topic is ever available
    }

    [Test]
    public void Should_delete_all_includes_content_of_relations()
    {
        RecycleContainer();
        ContextCategory.New().AddCaseThreeToCache();
        var rootCategoryOriginal = EntityCache.GetAllCategories().First().DeepClone();

        var rootCategoryCopy2 = rootCategoryOriginal.DeepClone();
        var rootCategoryCopy1 = rootCategoryOriginal.DeepClone();

        var result = GraphService.IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(true));

        rootCategoryCopy1.Name = "geändert";
        result = GraphService.IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(true));

        rootCategoryCopy1.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                CategoryId = 111
            }
        };

        rootCategoryCopy2.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                CategoryId = 111
            }
        };

        result = GraphService.IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(true));


        rootCategoryCopy2.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                CategoryId = 113
            }
        };

        result = GraphService.IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(false));

        rootCategoryCopy1.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                CategoryId = 111
            }
        };

        rootCategoryCopy2.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                CategoryId = 112
            }
        };

        result = GraphService.IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(false));
    }

    private bool IsAllRelationsAChildOf(IList<CategoryCacheRelation> categoryCacheRelations)
    {
        var result = true;
        foreach (var cr in categoryCacheRelations)
        {
            if (cr.CategoryRelationType != CategoryRelationType.IsChildCategoryOf)
                result = false;
        }

        return result; 
    }

    private bool HasCorrectParent(CategoryCacheItem category, string nameParent)
    {
        return category.CategoryRelations.Any(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == nameParent);
    }

   

    private bool IsCategoryRelationsCategoriesIdCorrect(CategoryCacheItem category)
    {
        return category.CategoryRelations.Select(cr =>EntityCache.GetCategoryCacheItem(cr.CategoryId).Name == category.Name).All(b => b);
    }

}


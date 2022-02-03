using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;
class GraphService_tests : BaseTest
{
    [Test, Sequential]
    public void Should_get_correct_category_with_relations()
    {
        var context = ContextCategory.New();

        var rootElement = context.Add("RootElement").Persist().All.First();

        var firstChildrenIds = context
            .Add("Sub1", parent: rootElement)
            .Persist()
            .All;

        var secondChildrenIds = context.Add("SubSub1", parent: firstChildrenIds.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");

        // Add User
        var user = ContextUser.New().Add("User").Persist(true, context).All[0];
        var userRoot = context.Add("Users Startseite").Persist().All.ByName("Users Startseite");

        CategoryInKnowledge.Pin(firstChildrenIds.ByName("SubSub1").Id, user);

        Sl.SessionUser.Login(user);
        EntityCache.Init();
        UserEntityCache.Init();

        var userRootCache = EntityCache.GetCategoryCacheItem(userRoot.Id);

        var userPersonnelCategoriesWithRelations = GraphService.GetAllWuwiWithRelations_TP(userRootCache, 2);

        Assert.That(userPersonnelCategoriesWithRelations.ByName("SubSub1").Name, Is.EqualTo("SubSub1"));
        Assert.That(
            EntityCache.GetCategoryCacheItem(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations
                .First().RelatedCategoryId).Name, Is.EqualTo("Users Startseite"));
        Assert.That(
            EntityCache.GetCategoryCacheItem(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations
                .First().CategoryId).Name, Is.EqualTo("SubSub1"));
        Assert.That(
            userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations.First().CategoryRelationType,
            Is.EqualTo(CategoryRelationType.IsChildOf));
    }

    [Test]
    public void Wish_knowledge_filter_simple_test()
    {
        // Case https://docs.google.com/drawings/d/1Wbne-XXmYkA578uSc6nY0mxz_s-pG8E9Q9flmgY2ZNY/

        var context = ContextCategory.New();

        var rootElement = context.Add("A").Persist().All.First();

        var firstChildrenIds = context
            .Add("B", parent: rootElement)
            .Add("C", parent: rootElement)
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

        var user = ContextUser.New().Add("User").Persist(true, context).All[0];

        var userRoot = context.Add("Users Startseite").Persist().All.ByName("Users Startseite");

        // Add in WUWI
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("C").Id, user);
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("G").Id, user);
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("E").Id, user);
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("I").Id, user);

        Sl.SessionUser.Login(user);

        EntityCache.Init();

        var userPersonalCategoriesWithRelations =
            GraphService.GetAllWuwiWithRelations_TP(CategoryCacheItem.ToCacheCategory(userRoot), user.Id);

        //Test C
        Assert.That(IsAllRelationsAChildOf(userPersonalCategoriesWithRelations.ByName("C").CategoryRelations),
            Is.EqualTo(false));

        Assert.That(userPersonalCategoriesWithRelations.ByName("C").CategoryRelations.First().RelatedCategoryId,
            Is.EqualTo(userRoot.Id));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("C").CategoryRelations
                .First()
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("C").Id));

        //Test I
        Assert.That(IsAllRelationsAChildOf(userPersonalCategoriesWithRelations.ByName("I").CategoryRelations),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectParent(userPersonalCategoriesWithRelations.ByName("I"), "C"),
            Is.EqualTo(true));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("I")
                .CategoryRelations
                .First()
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("I").Id));

        var relationId = userPersonalCategoriesWithRelations
            .ByName("I")
            .CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "E")
            .Select(cr => cr.RelatedCategoryId).First();

        Assert.That(relationId,
            Is.EqualTo(secondChildrenIds.ByName("E").Id));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("I")
                .CategoryRelations[1]
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("I").Id));

        relationId = userPersonalCategoriesWithRelations
            .ByName("I")
            .CategoryRelations.Where(cr => EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == "G")
            .Select(cr => cr.RelatedCategoryId).First();

        Assert.That(relationId,
            Is.EqualTo(secondChildrenIds.ByName("G").Id));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("I")
                .CategoryRelations[2]
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("I").Id));

        // Test G 
        Assert.That(userPersonalCategoriesWithRelations
                .ByName("G").CategoryRelations
                .First()
                .CategoryRelationType,
            Is.EqualTo(CategoryRelationType.IsChildOf));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("G")
                .CategoryRelations
                .First()
                .RelatedCategoryId,
            Is.EqualTo(firstChildrenIds.ByName("C").Id));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("G").CategoryRelations
                .First()
                .CategoryId,
            Is.EqualTo(secondChildrenIds.ByName("G").Id));

        // Test E
        Assert.That(userPersonalCategoriesWithRelations
                .ByName("E").CategoryRelations
                .First()
                .CategoryRelationType,
            Is.EqualTo(CategoryRelationType.IsChildOf));

        Assert.That(userPersonalCategoriesWithRelations
                .ByName("E")
                .CategoryRelations
                .First()
                .RelatedCategoryId,
            Is.EqualTo(firstChildrenIds.ByName("C").Id));

        Assert.That(userPersonalCategoriesWithRelations
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

        var userPersonalCategoriesWithRelations = GraphService.GetAllWuwiWithRelations_TP(rootElement, 2);

        var userWikiName = Sl.SessionUser.User.Name + "s Startseite";

        //Test I
        Assert.That(IsAllRelationsAChildOf(userPersonalCategoriesWithRelations.ByName("I").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonalCategoriesWithRelations.ByName("I")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonalCategoriesWithRelations.ByName("I"), "A"),
            Is.EqualTo(false));

        Assert.That(HasCorrectParent(userPersonalCategoriesWithRelations.ByName("I"), "E"),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonalCategoriesWithRelations.ByName("I"), "G"),
            Is.EqualTo(true));

        //Test B
        Assert.That(IsAllRelationsAChildOf(userPersonalCategoriesWithRelations.ByName("B").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonalCategoriesWithRelations.ByName("B")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonalCategoriesWithRelations.ByName("B"), userWikiName),
            Is.EqualTo(true));

        //Test E
        Assert.That(IsAllRelationsAChildOf(userPersonalCategoriesWithRelations.ByName("E").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonalCategoriesWithRelations.ByName("E")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonalCategoriesWithRelations.ByName("E"), userWikiName),
            Is.EqualTo(true));

        //Test G
        Assert.That(IsAllRelationsAChildOf(userPersonalCategoriesWithRelations.ByName("G").CategoryRelations)
            , Is.EqualTo(false));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonalCategoriesWithRelations.ByName("G")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonalCategoriesWithRelations.ByName("G"), userWikiName),
            Is.EqualTo(true));
    }

    [Test]
    public void Wish_knowledge_filter_complex_test()
    {
        var user = ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();

        var rootElement = EntityCache.GetAllCategories().ByName("A");
        Sl.SessionUser.Login(user);
        var userWikiName = Sl.SessionUser.User.Name + "s Startseite";

        var allPersonalCategoriesWithRelations = GraphService.GetAllWuwiWithRelations_TP(rootElement, 2);

        //Test I
        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("I").CategoryRelations),
            Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("I")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("I"), userWikiName),
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

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("G"), userWikiName),
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

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("F"), userWikiName),
            Is.EqualTo(false));

        //Test X3
        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("X3").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("X3")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("X3"), userWikiName),
            Is.EqualTo(true));

        Assert.That(IsAllRelationsAChildOf(allPersonalCategoriesWithRelations.ByName("X").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(allPersonalCategoriesWithRelations.ByName("X")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(allPersonalCategoriesWithRelations.ByName("X"), userWikiName),
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

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName(userWikiName), "B"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName(userWikiName), "X"),
            Is.EqualTo(true));

        Assert.That(ContextCategory.HasCorrectChild(allPersonalCategoriesWithRelations.ByName(userWikiName), "X3"),
            Is.EqualTo(true));

        Assert.That(allPersonalCategoriesWithRelations.ByName(userWikiName).CachedData.ChildrenIds.Count,
            Is.EqualTo(3));
    }

    [Test]
    public void Wish_knowledge_filter_special_case()
    {
        //docs.google.com/drawings/d/1CWJoFSk5aAJf1EOpWqf1Ffr6ncjwxpOcJqFWZEXUVk4

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

        var user = ContextUser.New().Add("User").Persist(true, context).All[0];
        var userRoot = context.Add("Users Startseite").Persist().All.ByName("Users Startseite");

        // Add in WUWI
        CategoryInKnowledge.Pin(firstChildrenIds.ByName("F").Id, user);

        Sl.SessionUser.Login(user);
        EntityCache.Init();
        UserEntityCache.Init();

        var userPersonnelCategoriesWithRelations =
            GraphService.GetAllWuwiWithRelations_TP(CategoryCacheItem.ToCacheCategory(rootElement), 2);

        //Test F
        Assert.That(IsAllRelationsAChildOf(userPersonnelCategoriesWithRelations.ByName("F").CategoryRelations)
            , Is.EqualTo(true));

        Assert.That(IsCategoryRelationsCategoriesIdCorrect(userPersonnelCategoriesWithRelations
                .ByName("F")),
            Is.EqualTo(true));

        Assert.That(HasCorrectParent(userPersonnelCategoriesWithRelations.ByName("F"), "Users Startseite"),
            Is.EqualTo(true));
        Assert.That(userPersonnelCategoriesWithRelations.ByName("F").CategoryRelations.Count, Is.EqualTo(1));
    }

    [Test]
    public void Without_wish_knowledge()
    {
        ContextCategory.New().AddCaseThreeToCache(false);
        EntityCache.Init();

        var rootElement = EntityCache.GetAllCategories().ByName("A");

        var userPersonelCategoriesWithRealtions = GraphService.GetAllWuwiWithRelations_TP(rootElement, 2);
        Assert.That(userPersonelCategoriesWithRealtions.Count, Is.EqualTo(1)); //root topic is ever available
    }

    [Test]
    public void Should_delete_all_includes_content_of_relations()
    {
        ContextCategory.New().AddCaseThreeToCache();
        var rootCategoryOriginal = EntityCache.GetAllCategories().First().DeepClone();

        var rootCategoryCopy2 = rootCategoryOriginal.DeepClone();
        var rootCategoryCopy1 = rootCategoryOriginal.DeepClone();

        var result = IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(true));

        rootCategoryCopy1.Name = "geändert";
        result = IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(true));

        rootCategoryCopy1.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildOf,
                CategoryId = 111
            }
        };

        rootCategoryCopy2.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildOf,
                CategoryId = 111
            }
        };

        result = IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(true));


        rootCategoryCopy2.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildOf,
                CategoryId = 113
            }
        };

        result = IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(false));

        rootCategoryCopy1.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildOf,
                CategoryId = 111
            }
        };

        rootCategoryCopy2.CategoryRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
            {
                RelatedCategoryId = 222,
                CategoryRelationType = CategoryRelationType.IsChildOf,
                CategoryId = 112
            }
        };

        result = IsCategoryRelationEqual(rootCategoryCopy1, rootCategoryCopy2);
        Assert.That(result, Is.EqualTo(false));
    }

    private bool IsAllRelationsAChildOf(IList<CategoryCacheRelation> categoryCacheRelations)
    {
        var result = true;
        foreach (var cr in categoryCacheRelations)
        {
            if (cr.CategoryRelationType != CategoryRelationType.IsChildOf)
                result = false;
        }

        return result;
    }

    private bool HasCorrectParent(CategoryCacheItem category, string nameParent)
    {
        return category.CategoryRelations.Any(cr =>
            EntityCache.GetCategoryCacheItem(cr.RelatedCategoryId).Name == nameParent);
    }

    private bool IsCategoryRelationsCategoriesIdCorrect(CategoryCacheItem category)
    {
        return category.CategoryRelations
            .Select(cr => EntityCache.GetCategoryCacheItem(cr.CategoryId).Name == category.Name).All(b => b);
    }

    private static bool IsCategoryRelationEqual(CategoryCacheItem category1, CategoryCacheItem category2)
    {
        if (category1 != null && category2 != null ||
            category1.CategoryRelations != null && category2.CategoryRelations != null)
        {
            var relations1 = category1.CategoryRelations;
            var relations2 = category2.CategoryRelations;

            if (relations2.Count != relations1.Count)
                return false;

            if (relations2.Count == 0 && relations1.Count == 0)
                return true;

            var count = 0;

            var countVariousRelations = relations1.Count(r => !relations2.Any(r2 =>
                r2.RelatedCategoryId == r.RelatedCategoryId && r2.CategoryId == r.CategoryId &&
                r2.CategoryRelationType.ToString().Equals(r.CategoryRelationType.ToString())));
            return countVariousRelations == 0;
        }

        Logg.r().Error("Category or CategoryRelations have a NullReferenceException");
        return false;
    }

    [Test]
    public void Should_get_parents_after_wuwi_toggle()
    {
        var context = ContextCategory.New();

        var parent = context.Add("parent").Persist().All.First();
        var user = ContextUser.New().Add("User").Persist(true, context).All[0];

        var userRoot = context.Add("Users Startseite", parent: parent).Persist().All.ByName("Users Startseite");

        Sl.SessionUser.Login(user);
        EntityCache.Clear();
        EntityCache.Init();

        var userRootCache = EntityCache.GetCategoryCacheItem(user.StartTopicId);

        Assert.That(HasCorrectParent(userRootCache, "parent"),
            Is.EqualTo(true));

        UserEntityCache.Init(user.Id);
        UserCache.GetItem(user.Id).IsFiltered = true;

        Assert.That(HasCorrectParent(userRootCache, "parent"),
            Is.EqualTo(false));

        UserCache.GetItem(user.Id).IsFiltered = false;

        Assert.That(HasCorrectParent(userRootCache, "parent"),
            Is.EqualTo(true));
    }

    private void AllToCache(List<Category> categories)
    {
        foreach (var cat in categories)
            CategoryCacheItem.ToCacheCategory(cat);
    }
}
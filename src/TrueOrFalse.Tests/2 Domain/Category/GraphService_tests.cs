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

        SessionUser.Login(user);
        EntityCache.Init();
        UserEntityCache.Init();

        var userRootCache = EntityCache.GetCategory(userRoot.Id);

        var userPersonnelCategoriesWithRelations = GraphService.GetAllWuwiWithRelations_TP(userRootCache, 2);

        Assert.That(userPersonnelCategoriesWithRelations.ByName("SubSub1").Name, Is.EqualTo("SubSub1"));
        Assert.That(
            EntityCache.GetCategory(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations
                .First().RelatedCategoryId).Name, Is.EqualTo("Users Startseite"));
        Assert.That(
            EntityCache.GetCategory(userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations
                .First().CategoryId).Name, Is.EqualTo("SubSub1"));
        Assert.That(
            userPersonnelCategoriesWithRelations.ByName("SubSub1").CategoryRelations.First().CategoryRelationType,
            Is.EqualTo(CategoryRelationType.IsChildOf));
    }

    [Test]
    public void Without_wish_knowledge()
    {
        ContextCategory.New().AddCaseThreeToCache();
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
            EntityCache.GetCategory(cr.RelatedCategoryId).Name == nameParent);
    }

    private bool IsCategoryRelationsCategoriesIdCorrect(CategoryCacheItem category)
    {
        return category.CategoryRelations
            .Select(cr => EntityCache.GetCategory(cr.CategoryId).Name == category.Name).All(b => b);
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

    private void AllToCache(List<Category> categories)
    {
        foreach (var cat in categories)
            CategoryCacheItem.ToCacheCategory(cat);
    }
}
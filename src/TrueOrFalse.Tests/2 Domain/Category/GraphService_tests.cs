﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;
class GraphService_tests : BaseTest
{
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
}
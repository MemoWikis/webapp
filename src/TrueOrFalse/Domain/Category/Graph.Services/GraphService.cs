using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class GraphService
{
    public static IList<CategoryCacheItem> GetAllParentsFromEntityCache(int categoryId) =>
        GetAllParentsFromEntityCache(EntityCache.GetCategory(categoryId));

    private static IList<CategoryCacheItem> GetAllParentsFromEntityCache(CategoryCacheItem category)
    {
        var parentIds = GetDirectParentIds(category);
        var allParents = new List<CategoryCacheItem>();
        var deletedIds = new Dictionary<int, int>();

        while (parentIds.Count > 0)
        {
            var parent = EntityCache.GetCategory(parentIds[0]);

            if (!deletedIds.ContainsKey(parentIds[0]))
            {
                allParents.Add(parent); //Avoidance of circular references

                deletedIds.Add(parentIds[0], parentIds[0]);
                var currentParents = GetDirectParentIds(parent);
                foreach (var currentParent in currentParents)
                {
                    parentIds.Add(currentParent);
                }
            }

            parentIds.RemoveAt(0);
        }

        return allParents;
    }

    private static List<int> GetDirectParentIdsInWuwi(CategoryCacheItem userEntityCacheItem)
    {
        var directParentIds = GetDirectParentIds(userEntityCacheItem);
        var directParentIdsInWuwi = EntityCache.GetCategories(directParentIds)
            .Where(cci => cci.IsInWishknowledge() || cci.Id == SessionUser.User.StartTopicId)
            .Select(cci => cci.Id).ToList();
        return directParentIdsInWuwi;
    }

    public static List<int> GetDirectParentIds(CategoryCacheItem category)
    {
        if (category == null)
        {
            return new List<int>();
        }

        return category.CategoryRelations
            .Select(cr => cr.RelatedCategoryId).ToList();
    }

    private static List<CategoryCacheItem> GetAllChildrenFromAllCategories(CategoryCacheItem rootCategory,
        CategoryCacheItem personalHomepage)
    {
        rootCategory.CategoryRelations.Add(new CategoryCacheRelation
        {
            CategoryId = rootCategory.Id,
            RelatedCategoryId = personalHomepage.Id
        });

        var wuwiChildren = EntityCache.GetAllCategories()
            .Where(c => c.IsInWishknowledge())
            .Distinct()
            .Select(c => c.DeepClone())
            .ToList();

        return wuwiChildren;
    }

    private static bool IsInWishknowledgeOrParentIsPersonalHomepage(bool isRootDirectParent, int userId, int parentId)
    {
        return SessionUserCache.IsInWishknowledge(userId, parentId) || isRootDirectParent;
    }

    public static ConcurrentDictionary<int, CategoryCacheItem> AddChildrenIdsToCategoryCacheData(
        ConcurrentDictionary<int, CategoryCacheItem> categories)
    {
        foreach (var category in categories.Values)
        {
            foreach (var categoryRelation in category.CategoryRelations)
            {
                if (categories.ContainsKey(categoryRelation.RelatedCategoryId))
                {
                    categories[categoryRelation.RelatedCategoryId].CachedData
                        .AddChildId(categories[categoryRelation.CategoryId].Id);
                }
            }
        }

        return categories;
    }

    public static void AutomaticInclusionOfChildCategoriesForEntityCacheAndDbUpdate(CategoryCacheItem category,
        IList<CategoryCacheItem> oldParents)
    {
        var parentsFromParentCategories = GetAllParentsFromEntityCache(category.Id);

        foreach (var oldParent in oldParents)
        {
            for (var i = oldParent.CategoryRelations.Count - 1; i > 0; i--)
            {
                if (oldParent.CategoryRelations[i].RelatedCategoryId == category.Id)
                    oldParent.CategoryRelations.RemoveAt(i);
            }
        }

        //foreach (var parentCategory in parentsFromParentCategories)
        //    ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(
        //        EntityCache.GetCategory(parentCategory.Id));
    }

    public static void AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(CategoryCacheItem category)
    {
        var parentsFromParentCategories = GetAllParentsFromEntityCache(category.Id);

        foreach (var parent in parentsFromParentCategories)
        {
            //var descendants = GetCategoryChildren.WithAppliedRules(parent, true);
            //var descendantsAsCategory = Sl.CategoryRepo.GetByIds(descendants.Select(cci => cci.Id).ToList());

            //var parentAsCategory = Sl.CategoryRepo.GetByIdEager(parent.Id);

            //var existingRelations =
            //    ModifyRelationsForCategory.GetExistingRelations(parentAsCategory).ToList();

            //var relationsToAdd = ModifyRelationsForCategory.GetRelationsToAdd(parentAsCategory,
            //    descendantsAsCategory,
            //    existingRelations);

            //ModifyRelationsForCategory.CreateIncludeContentOf(parentAsCategory, relationsToAdd);
            //Sl.CategoryRepo.Update(Sl.CategoryRepo.GetByIdEager(parent.Id), isFromModifiyRelations: true);

            parent.UpdateCountQuestionsAggregated();
        }
    }

    public static bool IsCategoryParentEqual(IList<CategoryCacheItem> parent1, IList<CategoryCacheItem> parent2)
    {
        if (parent1 == null || parent2 == null)
        {
            Logg.r().Error("parent1 or parent2 have a NullReferenceException");
            return false;
        }

        if (parent1.Count != parent2.Count)
            return false;

        if (parent1.Count == 0 && parent2.Count == 0)
            return true;

        var result = parent1.Where(p => !parent2.Any(p2 => p2.Id == p.Id)).Count();

        return result == 0;
    }
}
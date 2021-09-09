using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class GraphService
{
    public static IList<CategoryCacheItem> GetAllParentsFromEntityCache(int categoryId) =>
       GetAllParentsFromEntityCache(EntityCache.GetCategoryCacheItem(categoryId, getDataFromEntityCache: true));

    public static IList<CategoryCacheItem> GetAllParentsFromEntityCache(CategoryCacheItem category)
    {
        var parentIds = GetDirectParents(category);
        var allParents = new List<CategoryCacheItem>();
        var deletedIds = new Dictionary<int, int>();

        while (parentIds.Count > 0)
        {
            var parent = EntityCache.GetCategoryCacheItem(parentIds[0], getDataFromEntityCache: true);

            if (!deletedIds.ContainsKey(parentIds[0]))
            {
                allParents.Add(parent);//Avoidance of circular references

                deletedIds.Add(parentIds[0], parentIds[0]);
                var currentParents = GetDirectParents(parent);
                foreach (var currentParent in currentParents)
                {
                    parentIds.Add(currentParent);
                }
            }
            parentIds.RemoveAt(0);
        }
        return allParents;
    }

    public static List<CategoryCacheItem> GetAllParentsFromUserEntityCache(int userId, CategoryCacheItem category)
    {
       var userCache =  UserEntityCache.GetUserCache(userId);

       userCache.TryGetValue(category.Id, out var userEntityCacheItem);
       var parentIds = GetDirectParents(userEntityCacheItem);
       var allParents = new List<CategoryCacheItem>();
       var deletedIds = new Dictionary<int, int>();

       while (parentIds.Count > 0)
       { 
           userCache.TryGetValue(parentIds[0], out var parent);

           if (!deletedIds.ContainsKey(parentIds[0]))
           {
               allParents.Add(parent);//Avoidance of circular references

               deletedIds.Add(parentIds[0], parentIds[0]);
               var currentParents = GetDirectParents(parent);
               foreach (var currentParent in currentParents)
               {
                   parentIds.Add(currentParent);
               }
           }
           parentIds.RemoveAt(0);
       }
       return allParents;
    }

    public static List<int> GetDirectParents(CategoryCacheItem category)
    {
        var relations = new List<int>(); 
        foreach (var relation in category.CategoryRelations)
        {
            if(relation.CategoryRelationType == CategoryRelationType.IsChildOf)
                relations.Add(relation.RelatedCategoryId);
        }
        return category.CategoryRelations
            .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
            .Select(cr => cr.RelatedCategoryId).ToList();
    }

    public static IList<CategoryCacheItem> GetAllWuwiWithRelations_TP(CategoryCacheItem category, int userId = -1) =>
        GetAllWuwiCategoriesWithRelations(category.Id, userId, true);

    public static IList<CategoryCacheItem> GetAllWuwiCategoriesWithRelations(int rootCategoryId, int userId = -1, bool isFromUserEntityCache = false)
    {
        userId = userId == -1 ? Sl.CurrentUserId : userId;
        var rootCategory = EntityCache.GetCategoryCacheItem(rootCategoryId, isFromUserEntityCache).DeepClone();

        var personalStartsite = EntityCache.GetCategoryCacheItem(UserCache.GetUser(userId).StartTopicId, getDataFromEntityCache: true);
        var wuwiChildren = GetAllChildrenFromAllCategories(rootCategory, personalStartsite); 

        foreach (var wuwiChild in wuwiChildren)
        {
            var parents = GetParentsFromCategory(wuwiChild.Id, isFromUserEntityCache).ToList();
            var isRootDirectParent = parents.Contains(personalStartsite.Id); 
            wuwiChild.CategoryRelations.Clear();

            while (parents.Count > 0)
            {
                var parentId = parents.First();

                if (IsInWishknowledgeOrParentIsRoot(isRootDirectParent, userId, parentId))
                {
                    var categoryRelation = new CategoryCacheRelation
                    {
                        CategoryRelationType = CategoryRelationType.IsChildOf,
                        CategoryId = wuwiChild.Id,
                        RelatedCategoryId = parentId
                    };

                    if (!wuwiChild.Contains(categoryRelation))
                        wuwiChild.CategoryRelations.Add(categoryRelation);

                    parents.Remove(parentId);
                }
                else
                {
                    var currentParents = GetParentsFromCategory(parentId, isFromUserEntityCache);
                    parents.Remove(parentId);

                    foreach (var cp in currentParents)
                        parents.Add(cp);

                    parents = parents.Distinct().ToList();
                }
            }
        }

        foreach (var wuwiChild in wuwiChildren)
        {
            if (wuwiChild.CategoryRelations.Count == 0) 
            {
                wuwiChild.CategoryRelations.Add(new CategoryCacheRelation()
                {
                    CategoryRelationType = CategoryRelationType.IsChildOf,
                    RelatedCategoryId = personalStartsite.Id,
                    CategoryId = wuwiChild.Id
                });
            }
            wuwiChild.CachedData.ChildrenIds = new List<int>();
        }

        personalStartsite.CategoryRelations = new List<CategoryCacheRelation>();
        personalStartsite.CachedData.ChildrenIds = new List<int>();
        wuwiChildren.Add(personalStartsite);

        var wuwiChildrenDic = wuwiChildren.ToConcurrentDictionary();
        var cacheItemWithChildren = AddChildrenToCategory(wuwiChildrenDic);
        var noAddChildrenIds = new ConcurrentDictionary<int, int>();

        foreach (var categoryCacheItem in cacheItemWithChildren.Values)
        {
            var childrenOuter = categoryCacheItem.CachedData.ChildrenIds.DeepClone();

            while (childrenOuter.Count > 0)
            {
                wuwiChildrenDic.TryGetValue(childrenOuter[0], out var value);

                if (!noAddChildrenIds.ContainsKey(childrenOuter[0]))
                    categoryCacheItem.CategoryRelations.Add(new CategoryCacheRelation
                    {
                        CategoryRelationType = CategoryRelationType.IncludesContentOf,
                        RelatedCategoryId = value.Id,
                        CategoryId = categoryCacheItem.Id
                    });

                noAddChildrenIds.TryAdd(childrenOuter[0], childrenOuter[0]);
                childrenOuter.RemoveAt(0);

                foreach (var cachedDataChildrenId in value.CachedData.ChildrenIds)
                {
                    childrenOuter.Add(cachedDataChildrenId);
                }
            }
        }
        //UserCache.GetItem(userId).CategoryValuations.TryGetValue(RootCategory.RootCategoryId, out var rootCategoryValuation);
        //bool isRootinWuwi = rootCategoryValuation.RelevancePersonal > 1; 
       
        //if(!isRootinWuwi)
        //    ChangeFromRootCategoryToPersonalStartSite(userId, cacheItemWithChildren);
        //else
        //    AddRootTopicToPersonalStartsite(userId,cacheItemWithChildren);

        return cacheItemWithChildren.Values.ToList(); 
    }

    private static List<CategoryCacheItem> GetAllChildrenFromAllCategories(CategoryCacheItem rootCategory, CategoryCacheItem personalStartsite)
    {
        rootCategory.CategoryRelations.Add(new CategoryCacheRelation
        {
            CategoryId = rootCategory.Id,
            CategoryRelationType = CategoryRelationType.IsChildOf,
            RelatedCategoryId = personalStartsite.Id
        });

        var wuwiChildren = EntityCache.GetAllChildren(rootCategory.Id, true)
            .Distinct()
            .Where(c => c.IsInWishknowledge())
            .Select(c => c.DeepClone())
            .ToList();

        var wuwiChildren1 = EntityCache.GetAllChildren(personalStartsite.Id, true)
            .Distinct()
            .Where(c => c.IsInWishknowledge())
            .Select(c => c.DeepClone())
            .ToList();

        foreach (var wuwichild in wuwiChildren1)
        {
            wuwiChildren.Add(wuwichild);
        }
        wuwiChildren.Add(rootCategory);
        return wuwiChildren;
    }

    private static bool IsInWishknowledgeOrParentIsRoot(bool isRootDirectParent, int userId, int parentId)
    {
        return UserCache.IsInWishknowledge(userId, parentId) ||  isRootDirectParent;
    }

    private static void AddRootTopicToPersonalStartsite(int userId, ConcurrentDictionary<int, CategoryCacheItem> cacheItemWithChildren)
    {
        var personalStartTopic = EntityCache.GetCategoryCacheItem(UserCache.GetItem(userId).User.StartTopicId, getDataFromEntityCache: true);
        foreach (var categoryCacheItemId in cacheItemWithChildren.Keys)
        {
            personalStartTopic.CategoryRelations.Add(new CategoryCacheRelation
            {
                CategoryId = personalStartTopic.Id,
                CategoryRelationType = CategoryRelationType.IncludesContentOf,
                RelatedCategoryId = categoryCacheItemId
            });
        }
       
        cacheItemWithChildren.TryAdd(personalStartTopic.Id, personalStartTopic);
        cacheItemWithChildren.TryGetValue(RootCategory.RootCategoryId, out var rootCategory); 
        rootCategory.CategoryRelations.Add(new CategoryCacheRelation
        {
            CategoryId = rootCategory.Id,
            CategoryRelationType = CategoryRelationType.IsChildOf,
            RelatedCategoryId = personalStartTopic.Id
        });
    }

    private static void ChangeFromRootCategoryToPersonalStartSite(int userId, ConcurrentDictionary<int,CategoryCacheItem> cacheItemWithChildren)
    {
        var user = UserCache.GetItem(userId).User;
        var children = cacheItemWithChildren.Values
            .SelectMany(cci => cci.CategoryRelations
                .Where(cr => cr.RelatedCategoryId == RootCategory.RootCategoryId
                             && cr.CategoryRelationType == CategoryRelationType.IsChildOf));

        foreach (var categoryCacheRelation in children)
        {
            categoryCacheRelation.RelatedCategoryId = user.StartTopicId;
        }

        cacheItemWithChildren.TryGetValue(1, out var rootCategoryOld);
        foreach (var categoryRelation in rootCategoryOld.CategoryRelations)
        {
            categoryRelation.CategoryId = user.StartTopicId;
        }

        var personalStartSite = EntityCache.GetCategoryCacheItem(user.StartTopicId, getDataFromEntityCache: true);
        rootCategoryOld.Name = personalStartSite.Name;
        rootCategoryOld.Content = personalStartSite.Content;
        rootCategoryOld.Creator = user;
        rootCategoryOld.Id = personalStartSite.Id;
    }

    public static ConcurrentDictionary<int, CategoryCacheItem> AddChildrenToCategory(ConcurrentDictionary<int, CategoryCacheItem> categories)
    {
        foreach (var category in categories.Values)
        {
            foreach (var categoryRelation in category.CategoryRelations)
            {
                if (categoryRelation.CategoryRelationType == CategoryRelationType.IsChildOf && categories.ContainsKey(categoryRelation.RelatedCategoryId))
                {
                    categories[categoryRelation.RelatedCategoryId].CachedData.ChildrenIds
                        .Add(categories[categoryRelation.CategoryId].Id);
                }
            }
        }

        foreach (var category in categories)
        {
            category.Value.CachedData.ChildrenIds = category.Value.CachedData.ChildrenIds.Distinct().ToList();
        }
        return categories;
    }

    private static IEnumerable<int> GetParentsFromCategory(int categoryId, bool isFromUserEntityCache = false)
    {
        if (!isFromUserEntityCache)
        {
            var userCacheCategory = UserEntityCache.GetCategory(Sl.CurrentUserId, categoryId);
            return userCacheCategory.CategoryRelations
                .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
                .Select(cr => cr.RelatedCategoryId);
        }

        return EntityCache.GetCategoryCacheItem(categoryId, getDataFromEntityCache: true)
            .CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
            .Select(cr => cr.RelatedCategoryId);
    }

    public static void AutomaticInclusionOfChildCategoriesForEntityCacheAndDbUpdate(CategoryCacheItem category, IList<CategoryCacheItem> oldParents)
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

        foreach (var parentCategory in parentsFromParentCategories)
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(EntityCache.GetCategoryCacheItem(parentCategory.Id, getDataFromEntityCache: true));
    }

    public static void AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(CategoryCacheItem category)
    {
        var parentsFromParentCategories = GetAllParentsFromEntityCache(category.Id);

        foreach (var parent in parentsFromParentCategories)
        {
            var descendants = GetCategoryChildren.WithAppliedRules(parent, true);
            var descendantsAsCategory = Sl.CategoryRepo.GetByIds(descendants.Select(cci => cci.Id).ToList());

            var parentAsCategory = Sl.CategoryRepo.GetByIdEager(parent.Id);

            var existingRelations =
                ModifyRelationsForCategory.GetExistingRelations(parentAsCategory,
                    CategoryRelationType.IncludesContentOf).ToList();

            var relationsToAdd = ModifyRelationsForCategory.GetRelationsToAdd(parentAsCategory,
                descendantsAsCategory,
                CategoryRelationType.IncludesContentOf,
                existingRelations);

            ModifyRelationsForCategory.CreateIncludeContentOf(parentAsCategory, relationsToAdd);
            Sl.CategoryRepo.Update(Sl.CategoryRepo.GetByIdEager(parent.Id), isFromModifiyRelations: true);

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

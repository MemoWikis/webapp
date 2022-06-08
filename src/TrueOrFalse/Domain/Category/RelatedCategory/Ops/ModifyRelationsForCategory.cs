using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

public class ModifyRelationsForCategory
{
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="categoryCacheItem"></param>
    /// <param name="relatedCategorieIds">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public static void UpdateCategoryRelationsOfType(
        int categoryId,
        IList<int> relatedCategorieIds)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var relatedCategoriesAsCategories = Sl.CategoryRepo.GetByIdsEager(relatedCategorieIds);
        var existingRelationsOfType = GetExistingRelations(category).ToList();

        CreateIncludeContentOf(category, GetRelationsToAdd(category, relatedCategoriesAsCategories, existingRelationsOfType));
        RemoveIncludeContentOf(category, GetRelationsToRemove(relatedCategoriesAsCategories, existingRelationsOfType));
    }

    public static void AddCategoryRelationOfType(Category category, int relatedCategoryId, bool isRelatedParent = false)
    {
        var relatedCategory = Sl.CategoryRepo.GetByIdEager(relatedCategoryId);
        var categoryRelationToAdd = new CategoryRelation()
        {
            Category = category,
            RelatedCategory = relatedCategory,
        };
        if (!category.CategoryRelations.Any(cr => cr.Category == categoryRelationToAdd.Category && cr.RelatedCategory == categoryRelationToAdd.RelatedCategory))
        {
            category.CategoryRelations.Add(categoryRelationToAdd);
        }

        if (isRelatedParent) return;

        //Get all GrandChildren and add them to the CategoryRelations
        foreach (CategoryRelation categoryRelation in category.CategoryRelations)
        {
            AddCategoryRelationOfType(category, categoryRelation.RelatedCategory.Id, true);
        }

    }

    public static void AddParentCategory(Category child, int parent)
    {
        AddCategoryRelationOfType(child, parent);
    }

    public static void AddParentCategories(Category category, List<int> relatedCategoryIds)
    {
        foreach (var relatedId in relatedCategoryIds)
        {
            AddCategoryRelationOfType(category, relatedId);
        }
    }

    public static void UpdateRelationsOfTypeIncludesContentOf(CategoryCacheItem categoryCacheItem)
    {
        var allChildren = GetCategoryChildren.WithAppliedRules(categoryCacheItem);
        var allChildrenAsId = allChildren.Select(cci => cci.Id).ToList();

        UpdateCategoryRelationsOfType(categoryCacheItem.Id, allChildrenAsId);

        categoryCacheItem.UpdateCountQuestionsAggregated();
        Sl.CategoryRepo.Update(Sl.CategoryRepo.GetByIdEager(categoryCacheItem.Id), isFromModifiyRelations: true, author: SessionUser.User, type: CategoryChangeType.Relations, createCategoryChange: false);
    }

    public static IEnumerable<CategoryRelation> GetExistingRelations(Category category)
    {
        return category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.RelatedCategory.Id == category.Id).ToList()
            : new List<CategoryRelation>();
    }

    public static IEnumerable<CategoryRelation> GetRelationsToAdd(Category category,
        IEnumerable<Category> relatedCategoriesAsCategories,
        IEnumerable<CategoryRelation> existingRelationsOfType)
    {
        return relatedCategoriesAsCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(c => new CategoryRelation
            {
                Category = category,
                RelatedCategory = c,
            });
    }

    private static IEnumerable<CategoryRelation> GetRelationsToRemove(IList<Category> relatedCategoriesAsCategories, IEnumerable<CategoryRelation> existingRelationsOfType)
    {
        var relationsToRemove = new List<CategoryRelation>();
        var relatedCategoriesDictionary = relatedCategoriesAsCategories.ToConcurrentDictionary();

        foreach (var categoryRelation in existingRelationsOfType)
            if (!relatedCategoriesDictionary.ContainsKey(categoryRelation.RelatedCategory.Id))
                relationsToRemove.Add(categoryRelation);

        return relationsToRemove;
    }

    public static void CreateIncludeContentOf(Category category, IEnumerable<CategoryRelation> relationsToAdd)
    {
        foreach (var relation in relationsToAdd)
        {
            category.CategoryRelations.Add(relation);
            var categoryCacheItem = EntityCache.GetCategory(category.Id);
            categoryCacheItem.CategoryRelations.Add(new CategoryCacheRelation
            {
                CategoryId = relation.Category.Id,
                RelatedCategoryId = relation.RelatedCategory.Id
            });
        }
    }

    public static void RemoveIncludeContentOf(Category category, IEnumerable<CategoryRelation> relationsToRemove)
    {
        var relationsToRemoveList = relationsToRemove.ToList();

        if (category.CategoryRelations.Count < 2)
            return;

        for (var i = 0; i < relationsToRemoveList.Count; i++)
        {
            for (int j = 0; j < category.CategoryRelations.Count; j++)
            {
                if (relationsToRemoveList[i] == category.CategoryRelations[j])
                {
                    category.CategoryRelations.RemoveAt(j);
                    var categoryCacheItem = EntityCache.GetCategory(category.Id);
                    if (categoryCacheItem.CategoryRelations.Count >= j)
                        categoryCacheItem.CategoryRelations.RemoveAt(j);
                }
            }
        }
    }

    public static void RemoveRelation(Category category, Category relatedCategory)
    {
        for (int i = 0; i < category.CategoryRelations.Count; i++)
        {
            var relation = category.CategoryRelations[i];
            if (relation.Category.Id == category.Id &&
                relation.RelatedCategory.Id == relatedCategory.Id)
            {
                category.CategoryRelations.RemoveAt(i);
                break;
            }
        }
    }

    public static bool RemoveChildCategoryRelation(int parentCategoryIdToRemove, int childCategoryId)
    {
        var childCategory = EntityCache.GetCategory(childCategoryId);
        var parentCategories = childCategory.ParentCategories().Where(c => c.Id != parentCategoryIdToRemove);
        var parentCategoryAsCategory = Sl.CategoryRepo.GetById(parentCategoryIdToRemove);

        if (!childCategory.IsStartPage() && !CheckParentAvailability(parentCategories, childCategory))
            return false;

        if (!PermissionCheck.CanEdit(childCategory))
            throw new SecurityException("Not allowed to edit category");

        var childCategoryAsCategory = Sl.CategoryRepo.GetById(childCategory.Id);

        RemoveRelation(
            childCategoryAsCategory,
            parentCategoryAsCategory);

        RemoveRelation(
            parentCategoryAsCategory,
            childCategoryAsCategory);

        ModifyRelationsEntityCache.RemoveRelation(
            childCategory,
            parentCategoryIdToRemove);

        ModifyRelationsEntityCache.RemoveRelation(
            EntityCache.GetCategory(parentCategoryIdToRemove),
            childCategoryId);

        UserEntityCache.ReInitAllActiveCategoryCaches();

        return true;
    }

    private static bool CheckParentAvailability(IEnumerable<CategoryCacheItem> parentCategories, CategoryCacheItem childCategory)
    {
        var allParentsArePrivate = parentCategories.All(c => c.Visibility != CategoryVisibility.All);
        var childIsPublic = childCategory.Visibility == CategoryVisibility.All;

        if (!parentCategories.Any() || allParentsArePrivate && childIsPublic)
            return false;

        return true;
    }
}

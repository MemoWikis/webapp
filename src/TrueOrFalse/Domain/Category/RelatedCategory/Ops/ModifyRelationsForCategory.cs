﻿using System.Collections.Generic;
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
    }

    public static void AddParentCategory(Category category, int parentId)
    {
        var relatedCategory = Sl.CategoryRepo.GetByIdEager(parentId);
        var categoryRelationToAdd = new CategoryRelation()
        {
            Category = category,
            RelatedCategory = relatedCategory,
        };
        if (!category.CategoryRelations.Any(cr =>
                cr.Category == categoryRelationToAdd.Category &&
                cr.RelatedCategory == categoryRelationToAdd.RelatedCategory))
        {
            category.CategoryRelations.Add(categoryRelationToAdd);
        }
    }

    public static IEnumerable<CategoryRelation> GetExistingRelations(Category category)
    {
        return category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.RelatedCategory.Id == category.Id).ToList()
            : new List<CategoryRelation>();
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

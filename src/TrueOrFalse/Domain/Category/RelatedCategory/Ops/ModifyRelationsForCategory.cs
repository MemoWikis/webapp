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
        IList<int> relatedCategorieIds,
        CategoryRelationType relationType)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var relatedCategoriesAsCategories = Sl.CategoryRepo.GetByIdsEager(relatedCategorieIds);
        var existingRelationsOfType = GetExistingRelations(category, relationType).ToList();

        CreateIncludeContentOf(category, GetRelationsToAdd(category, relatedCategoriesAsCategories, relationType, existingRelationsOfType));
        RemoveIncludeContentOf(category, GetRelationsToRemove(relatedCategoriesAsCategories, existingRelationsOfType));
    }

    public static void AddCategoryRelationOfType(Category category, int relatedCategoryId, CategoryRelationType relationType, bool isRelatedParent = false)
    {
        var relatedCategory = Sl.CategoryRepo.GetByIdEager(relatedCategoryId);
        var categoryRelationToAdd = new CategoryRelation()
        {
            Category = category,
            RelatedCategory = relatedCategory,
            CategoryRelationType = relationType
        };
        if (!category.CategoryRelations.Any(cr => cr.Category == categoryRelationToAdd.Category && cr.RelatedCategory == categoryRelationToAdd.RelatedCategory))
        {
            category.CategoryRelations.Add(categoryRelationToAdd);
        }

        if (relationType != CategoryRelationType.IncludesContentOf)
        {

            //Get all GrandChildren and add them to the CategoryRelations
            if (isRelatedParent == false)
            {
                foreach (CategoryRelation categoryRelation in category.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf))
                {
                    AddCategoryRelationOfType(category, categoryRelation.RelatedCategory.Id, CategoryRelationType.IncludesContentOf, true);
                }
            }
        }

        else
        {
            //Get all Parents and add RelatedCategories to them
            foreach (CategoryRelation relatedCategoryRelation in category.CategoryRelations.Where(cr =>
                cr.CategoryRelationType == CategoryRelationType.IsChildOf).ToList())
            {
                foreach (CategoryRelation categoryRelation in category.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf).ToList())
                {
                    if (relatedCategoryRelation.RelatedCategory.CategoryRelations.All(cr => cr.RelatedCategory != categoryRelation.RelatedCategory))
                    {
                        AddCategoryRelationOfType(relatedCategoryRelation.RelatedCategory, categoryRelation.RelatedCategory.Id, CategoryRelationType.IncludesContentOf, true);
                    }
                }
            }
        }
    }

    public static void AddParentCategory(Category child, int parent)
    {
        AddCategoryRelationOfType(child, parent, CategoryRelationType.IsChildOf);
    }

    public static void AddParentCategories(Category category, List<int> relatedCategoryIds)
    {
        foreach (var relatedId in relatedCategoryIds)
        {
            AddCategoryRelationOfType(category, relatedId, CategoryRelationType.IsChildOf);
        }
    }

    public static void UpdateRelationsOfTypeIncludesContentOf(CategoryCacheItem categoryCacheItem)
    {
        var allChildren = GetCategoryChildren.WithAppliedRules(categoryCacheItem);
        var allChildrenAsId = allChildren.Select(cci => cci.Id).ToList();

        UpdateCategoryRelationsOfType(categoryCacheItem.Id, allChildrenAsId, CategoryRelationType.IncludesContentOf);

        categoryCacheItem.UpdateCountQuestionsAggregated();
        Sl.CategoryRepo.Update(Sl.CategoryRepo.GetByIdEager(categoryCacheItem.Id), isFromModifiyRelations: true, author: Sl.SessionUser.User, type: CategoryChangeType.Relations);
    }

    public static IEnumerable<CategoryRelation> GetExistingRelations(Category category, CategoryRelationType relationType)
    {
        return category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryRelation>();
    }

    public static IEnumerable<CategoryRelation> GetRelationsToAdd(Category category,
        IEnumerable<Category> relatedCategoriesAsCategories,
        CategoryRelationType relationType,
        IEnumerable<CategoryRelation> existingRelationsOfType)
    {
        return relatedCategoriesAsCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(c => new CategoryRelation
            {
                Category = category,
                RelatedCategory = c,
                CategoryRelationType = relationType
            }
            );
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
            var categoryCacheItem = EntityCache.GetCategoryCacheItem(category.Id);
            categoryCacheItem.CategoryRelations.Add(new CategoryCacheRelation
            {
                CategoryRelationType = relation.CategoryRelationType,
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
                    var categoryCacheItem = EntityCache.GetCategoryCacheItem(category.Id);
                    if (categoryCacheItem.CategoryRelations.Count >= j)
                        categoryCacheItem.CategoryRelations.RemoveAt(j);
                }
            }
        }
    }

    public static void RemoveRelation(Category category, Category relatedCategory, CategoryRelationType categoryRelationType)
    {
        for (int i = 0; i < category.CategoryRelations.Count; i++)
        {
            var relation = category.CategoryRelations[i];
            if (relation.Category.Id == category.Id &&
                relation.RelatedCategory.Id == relatedCategory.Id &&
                relation.CategoryRelationType == categoryRelationType)
            {
                category.CategoryRelations.RemoveAt(i);
                break;
            }
        }
    }

    public static bool RemoveChildCategoryRelation(int parentCategoryIdToRemove, int childCategoryId)
    {
        var childCategory = EntityCache.GetCategoryCacheItem(childCategoryId);
        var parentCategories = childCategory.ParentCategories().Where(c => c.Id != parentCategoryIdToRemove);
        var parentCategoryAsCategory = Sl.CategoryRepo.GetById(parentCategoryIdToRemove);

        if (!childCategory.IsStartPage() && CheckParentAvailability(parentCategories, childCategory))
            return false;

        if (!PermissionCheck.CanEdit(childCategory))
            throw new SecurityException("Not allowed to edit category");

        var childCategoryAsCategory = Sl.CategoryRepo.GetById(childCategory.Id);

        RemoveRelation(
            childCategoryAsCategory,
            parentCategoryAsCategory,
            CategoryRelationType.IsChildOf);

        RemoveRelation(
            parentCategoryAsCategory,
            childCategoryAsCategory,
            CategoryRelationType.IncludesContentOf);

        ModifyRelationsEntityCache.RemoveRelation(
            childCategory,
            parentCategoryIdToRemove,
            CategoryRelationType.IsChildOf);

        ModifyRelationsEntityCache.RemoveRelation(
            EntityCache.GetCategoryCacheItem(parentCategoryIdToRemove),
            childCategoryId,
            CategoryRelationType.IncludesContentOf);

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

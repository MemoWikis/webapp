using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client.Framing.Impl;

public class ModifyRelationsForCategory
{
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="categoryCacheItem"></param>
    /// <param name="relatedCategories">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public static void UpdateCategoryRelationsOfType(
        int categoryId,
        IList<int> relatedCategories, 
        CategoryRelationType relationType)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var relatedCategoriesAsCategories = Sl.CategoryRepo.GetByIdsEager(relatedCategories);
        var existingRelationsOfType = GetExistingRelations(category, relationType).ToList();
        


        CreateIncludeContentOf(category, GetRelationsToAdd(category, relatedCategoriesAsCategories, relationType, existingRelationsOfType));

        RemoveIncludeContentOf(category, GetRelationsToRemove(relatedCategoriesAsCategories, existingRelationsOfType)); 
    }

    private static void AddCategoryRelationOfType(Category category, int relatedCategoryId, CategoryRelationType relationType)
    {
        if(category.CategoryRelations.Any(r => r.RelatedCategory.Id == relatedCategoryId && r.CategoryRelationType == relationType))
            return;

        category.CategoryRelations.Add(
            new CategoryRelation()
            {
                Category = category,
                RelatedCategory = Sl.CategoryRepo.GetByIdEager(relatedCategoryId),
                CategoryRelationType = relationType 
            });
    }

    public static void AddParentCategory(Category category, int relatedCategoryId)
    {
        AddCategoryRelationOfType(category, relatedCategoryId, CategoryRelationType.IsChildOf);
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
        var descendants = GetCategoryChildren.WithAppliedRules(categoryCacheItem);
        var descendantsAsCategory = descendants.Select(cci => cci.Id).ToList();
        
        UpdateCategoryRelationsOfType(categoryCacheItem.Id, descendantsAsCategory, CategoryRelationType.IncludesContentOf);

        categoryCacheItem.UpdateCountQuestionsAggregated();
        Sl.CategoryRepo.Update(Sl.CategoryRepo.GetByIdEager(categoryCacheItem.Id), isFromModifiyRelations: true);
    }

    public static IEnumerable<CategoryRelation> GetExistingRelations(Category category, CategoryRelationType relationType)
    {
        return category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryRelation>();
    }

    public static IEnumerable<CategoryRelation> GetRelationsToAdd(Category category, IEnumerable<Category> relatedCategoriesAsCategories, CategoryRelationType relationType, IEnumerable<CategoryRelation> existingRelationsOfType)
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

    private static IEnumerable<CategoryRelation> GetRelationsToRemove( IList<Category> relatedCategoriesAsCategories, IEnumerable<CategoryRelation> existingRelationsOfType)
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
        for (var i = 0; i > relationsToRemove.Count(); i++)
        {
            category.CategoryRelations.RemoveAt(i);

            var categoryCacheItem = EntityCache.GetCategoryCacheItem(category.Id); 


            categoryCacheItem.CategoryRelations.RemoveAt(i);


        }
    }
}

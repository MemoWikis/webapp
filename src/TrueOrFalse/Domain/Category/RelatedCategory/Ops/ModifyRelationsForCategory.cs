using System;
using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsForCategory
{
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="categoryCacheItem"></param>
    /// <param name="relatedCategories">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public static void UpdateCategoryRelationsOfType(
        CategoryCacheItem categoryCacheItem,
        IList<int> relatedCategories, 
        CategoryRelationType relationType)
    {
        var existingRelationsOfType = categoryCacheItem.CategoryRelations.Any()
            ? categoryCacheItem.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryCacheRelation>();

        var relationsToAdd = relatedCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategoryId))
            .Select(Id => new CategoryCacheRelation{
                CategoryId = categoryCacheItem.Id, 
                RelatedCategoryId = Id, 
                CategoryRelationType = relationType}
        );

        var relationsToRemove = new List<CategoryCacheRelation>(); 
        var relatedCategoriesDictionary =  relatedCategories.ToDictionary(Id => Id);
        foreach (var categoryRelation in existingRelationsOfType)
            if (!relatedCategoriesDictionary.ContainsKey(categoryRelation.RelatedCategoryId))
            {
                relationsToRemove.Add(categoryRelation);
            }
        
        foreach (var relation in relationsToAdd)
            categoryCacheItem.CategoryRelations.Add(relation);

        foreach (var relation in relationsToRemove)
            categoryCacheItem.CategoryRelations.Remove(relation);
    }

    private static void AddCategoryRelationOfType(CategoryCacheItem categoryCacherCacheItem, int relatedCategoryId, CategoryRelationType relationType)
    {
        if(categoryCacherCacheItem.CategoryRelations.Any(r => r.RelatedCategoryId == relatedCategoryId && r.CategoryRelationType == relationType))
            return;

        categoryCacherCacheItem.CategoryRelations.Add(
            new CategoryCacheRelation()
            {
                CategoryId = categoryCacherCacheItem.Id,
                RelatedCategoryId = relatedCategoryId,
                CategoryRelationType = relationType 
            });
    }

    public static void AddParentCategory(CategoryCacheItem category, int relatedCategoryId)
    {
        AddCategoryRelationOfType(category, relatedCategoryId, CategoryRelationType.IsChildCategoryOf);
    }

    public static void UpdateRelationsOfTypeIncludesContentOf(CategoryCacheItem categoryCacheItem, bool persist = true)
    {
        var descendants = GetCategoryChildren.WithAppliedRules(categoryCacheItem);
        var descendantsAsCategory = descendants.Select(cci => cci.Id).ToList();
        

        UpdateCategoryRelationsOfType(categoryCacheItem, descendantsAsCategory, CategoryRelationType.IncludesContentOf);

        if(!persist)
            return;

        categoryCacheItem.UpdateCountQuestionsAggregated();
        Sl.CategoryRepo.Update(Sl.CategoryRepo.GetByIdEager(categoryCacheItem.Id), isFromModifiyRelations: true);

        KnowledgeSummaryUpdate.RunForCategory(categoryCacheItem.Id);
    }
}

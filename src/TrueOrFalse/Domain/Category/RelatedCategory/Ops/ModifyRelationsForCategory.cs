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
        var category = Sl.CategoryRepo.GetByIdEager(categoryCacheItem.Id);
        var relatedCategoriesAsCategories = Sl.CategoryRepo.GetByIdsEager(relatedCategories); 

        var existingRelationsOfType = category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryRelation>();

        var relationsToAdd = relatedCategoriesAsCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(Id => new CategoryRelation{
                Category = category, 
                RelatedCategory = Id, 
                CategoryRelationType = relationType}
        );

        var relationsToRemove = new List<CategoryRelation>(); 
        var relatedCategoriesDictionary =  relatedCategoriesAsCategories.ToConcurrentDictionary();
        foreach (var categoryRelation in existingRelationsOfType)
            if (!relatedCategoriesDictionary.ContainsKey(categoryRelation.RelatedCategory.Id))
            {
                relationsToRemove.Add(categoryRelation);
            }
        
        foreach (var relation in relationsToAdd)
            category.CategoryRelations.Add(relation);

        foreach (var relation in relationsToRemove)
            category.CategoryRelations.Remove(relation);
    }

    private static void AddCategoryRelationOfType(Category categoryCacherCacheItem, int relatedCategoryId, CategoryRelationType relationType)
    {
        if(categoryCacherCacheItem.CategoryRelations.Any(r => r.RelatedCategory.Id == relatedCategoryId && r.CategoryRelationType == relationType))
            return;

        categoryCacherCacheItem.CategoryRelations.Add(
            new CategoryRelation()
            {
                Category = categoryCacherCacheItem,
                RelatedCategory = Sl.CategoryRepo.GetByIdEager(relatedCategoryId),
                CategoryRelationType = relationType 
            });
    }

    public static void AddParentCategory(Category category, int relatedCategoryId)
    {
        AddCategoryRelationOfType(category, relatedCategoryId, CategoryRelationType.IsChildCategoryOf);
    }

    public static void AddParentCategories(Category category, List<int> relatedCategoryIds)
    {
        foreach (var relatedId in relatedCategoryIds)
        {
            AddCategoryRelationOfType(category, relatedId, CategoryRelationType.IsChildCategoryOf);
        }
        
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

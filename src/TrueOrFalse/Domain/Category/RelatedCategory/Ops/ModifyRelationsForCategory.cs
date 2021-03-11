using System;
using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsForCategory
{
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="category"></param>
    /// <param name="relatedCategories">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public static void UpdateCategoryRelationsOfType(
        Category category,
        IList<Category> relatedCategories, 
        CategoryRelationType relationType)
    {
        var existingRelationsOfType = category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryRelation>();

        var relationsToAdd = relatedCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(c => new CategoryRelation{
                Category = EntityCache.GetCategory(category.Id), 
                RelatedCategory = EntityCache.GetCategory(c.Id, getDataFromEntityCache: true), 
                CategoryRelationType = relationType}
        );

        var relationsToRemove = new List<CategoryRelation>(); 
        var relatedCategoriesDictionary =  relatedCategories.ToDictionary(r => r.Id);
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

    public static void AddCategoryRelationOfType(Category category, Category relatedCategory, CategoryRelationType relationType)
    {
        if(category.CategoryRelations.Any(r => r.RelatedCategory == relatedCategory && r.CategoryRelationType == relationType))
            return;

        category.CategoryRelations.Add(
            new CategoryRelation
            {
                Category = category,
                RelatedCategory = relatedCategory,
                CategoryRelationType = relationType 
            });
    }

    public static void AddParentCategory(Category category, Category relatedCategory)
    {
        AddCategoryRelationOfType(category, relatedCategory, CategoryRelationType.IsChildCategoryOf);
    }

    public static void UpdateRelationsOfTypeIncludesContentOf(Category category, bool persist = true)
    {
        var descendants = GetCategoryChildren.WithAppliedRules(category);
        UpdateCategoryRelationsOfType(category, descendants, CategoryRelationType.IncludesContentOf);

        if(!persist)
            return;

        category.UpdateCountQuestionsAggregated();
        Sl.CategoryRepo.Update(category, isFromModifiyRelations: true);

        KnowledgeSummaryUpdate.RunForCategory(category.Id);
    }
}

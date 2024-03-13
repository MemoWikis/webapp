using System.Collections.Generic;
using System.Linq;
using System.Security;

public class ModifyRelationsForCategory
{
    private readonly CategoryRepository _categoryRepository;
    private readonly CategoryRelationRepo _categoryRelationRepo;

    public ModifyRelationsForCategory(CategoryRepository categoryRepository, CategoryRelationRepo categoryRelationRepo)
    {
        _categoryRepository = categoryRepository;
        _categoryRelationRepo = categoryRelationRepo;
    }
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="categoryCacheItem"></param>
    /// <param name="relatedCategorieIds">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public void UpdateCategoryRelationsOfType(
        int categoryId,
        IList<int> relatedCategorieIds)
    {
        var category = _categoryRepository.GetByIdEager(categoryId);

    }

    public void AddParentCategory(Category category, int parentId)
    {
        var relatedCategory = _categoryRepository.GetByIdEager(parentId);
        var previousCachedRelation = EntityCache.GetCategory(parentId).ChildRelations.LastOrDefault();

        if (previousCachedRelation != null)
        {
            var previousRelation = _categoryRelationRepo.GetById(previousCachedRelation.Id);
            previousRelation.NextId = category.Id;

            _categoryRelationRepo.Update(previousRelation);
        }

        var categoryRelationToAdd = new CategoryRelation()
        {
            Child = category,
            Parent = relatedCategory,
            PreviousId = previousCachedRelation?.ChildId
        };

        _categoryRelationRepo.Create(categoryRelationToAdd);
    }

    public bool RemoveChildCategoryRelation(int parentCategoryIdToRemove, int childCategoryId, PermissionCheck permissionCheck)
    {
        var childCategory = EntityCache.GetCategory(childCategoryId);
        var newParentRelationsIds = childCategory.ParentRelations.Where(r => r.ParentId != parentCategoryIdToRemove).Select(r => r.ParentId);
        var parentCategories = EntityCache.GetCategories(newParentRelationsIds);

        if (!childCategory.IsStartPage() && !CheckParentAvailability(parentCategories, childCategory))
            return false;

        if (!permissionCheck.CanEdit(childCategory) && !permissionCheck.CanEditCategory(parentCategoryIdToRemove))  
            throw new SecurityException("Not allowed to edit category");

        var relationIdToRemove = ModifyRelationsEntityCache.RemoveParent(
            childCategory,
            parentCategoryIdToRemove);
        
        _categoryRelationRepo.Delete(relationIdToRemove);

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

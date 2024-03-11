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
        var relatedCategoriesAsCategories = _categoryRepository.GetByIdsEager(relatedCategorieIds);
        var existingRelationsOfType = GetExistingRelations(category).ToList();
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

    public static IEnumerable<CategoryRelation> GetExistingRelations(Category category)
    {
        return category.ParentRelations.Any()
            ? category.ParentRelations?.Where(r => r.Parent.Id == category.Id).ToList()
            : new List<CategoryRelation>();
    }

    public static void RemoveRelation(Category child, Category parent)
    {
        for (int i = 0; i < child.ParentRelations.Count; i++)
        {
            var relation = child.ParentRelations[i];
            if (relation.Child.Id == child.Id &&
                relation.Parent.Id == parent.Id)
            {
                child.ParentRelations.RemoveAt(i);
                break;
            }
        }
    }

    public bool RemoveChildCategoryRelation(int parentCategoryIdToRemove, int childCategoryId, PermissionCheck permissionCheck)
    {
        var childCategory = EntityCache.GetCategory(childCategoryId);
        var newParentRelationsIds = childCategory.ParentRelations.Where(r => r.ParentId != parentCategoryIdToRemove).Select(r => r.ParentId);
        var parentCategories = EntityCache.GetCategories(newParentRelationsIds);
        var parentCategoryAsCategory = _categoryRepository.GetById(parentCategoryIdToRemove);

        if (!childCategory.IsStartPage() && !CheckParentAvailability(parentCategories, childCategory))
            return false;

        if (!permissionCheck.CanEdit(childCategory) && !permissionCheck.CanEditCategory(parentCategoryIdToRemove))  
            throw new SecurityException("Not allowed to edit category");

        var childCategoryAsCategory = _categoryRepository.GetById(childCategory.Id);

        RemoveRelation(
            childCategoryAsCategory,
            parentCategoryAsCategory);

        ModifyRelationsEntityCache.RemoveParent(
            childCategory,
            parentCategoryIdToRemove);

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

    //public void MoveTopic(int topicId, int oldParentId, int newParentId)
    //{
    //    var topic = _categoryRepository.GetById(topicId);
    //    AddParentCategory(topic, newParentId);
    //    var oldParent = _categoryRepository.GetById(oldParentId);
    //    RemoveRelation(oldParent, topic);
    //}
}

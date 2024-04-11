﻿public class ModifyRelationsForCategory
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

    public void AddChild(int parentId, int childId)
    {
        var cachedParent = EntityCache.GetCategory(parentId);
        var previousCacheRelation = cachedParent.ChildRelations.LastOrDefault();

        var child = _categoryRepository.GetById(childId);
        var parent = _categoryRepository.GetById(parentId);

        var relation = new CategoryRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = previousCacheRelation != null ? previousCacheRelation.ChildId : null,
            NextId = null,
        };

        _categoryRelationRepo.Create(relation);

        if (previousCacheRelation != null)
        {
            var previousRelation = _categoryRelationRepo.GetById(previousCacheRelation.Id);
            previousRelation.NextId = childId;

            _categoryRelationRepo.Update(previousRelation);
        }

        ModifyRelationsEntityCache.AddChild(relation);
    }

    public int CreateNewRelationAndGetId(int parentId, int childId, int? nextId, int? previousId)
    {
        var child = _categoryRepository.GetById(childId);
        var parent = _categoryRepository.GetById(parentId);

        var relation = new CategoryRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = previousId,
            NextId = nextId,
        };

        _categoryRelationRepo.Create(relation);
        return relation.Id;
    }

    public void UpdateRelationsInDb(List<CategoryCacheRelation> cachedRelations, int authorId)
    {
        foreach (var r in cachedRelations)
        {
            Logg.r.Information("Job started - ModifyRelations RelationId: {relationId}, Child: {childId}, Parent: {parentId}", r.Id, r.ChildId, r.ParentId);

            var relationToUpdate = r.Id > 0 ? _categoryRelationRepo.GetById(r.Id) : null;
            var child = _categoryRepository.GetById(r.ChildId);
            var parent = _categoryRepository.GetById(r.ParentId);

            if (relationToUpdate != null)
            {
                relationToUpdate.Child = child;
                relationToUpdate.Parent = parent;
                relationToUpdate.PreviousId = r.PreviousId;
                relationToUpdate.NextId = r.NextId;

                _categoryRelationRepo.Update(relationToUpdate);
            }
            else
            {
                var relation = new CategoryRelation
                {
                    Child = child,
                    Parent = parent,
                    PreviousId = r.PreviousId,
                    NextId = r.NextId,
                };

                _categoryRelationRepo.Create(relation);
            }
            _categoryRepository.Update(child, authorId, type: CategoryChangeType.Relations);
            _categoryRepository.Update(parent, authorId, type: CategoryChangeType.Relations);
        }
    }
}

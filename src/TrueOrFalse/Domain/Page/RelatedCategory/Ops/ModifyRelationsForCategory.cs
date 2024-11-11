public class ModifyRelationsForCategory(
    PageRepository pageRepository,
    PageRelationRepo pageRelationRepo)
{
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="categoryCacheItem"></param>
    /// <param name="relatedCategorieIds">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public void UpdateCategoryRelationsOfType(
        int pageId,
        IList<int> relatedCategorieIds)
    {
        var category = pageRepository.GetByIdEager(pageId);
    }

    public void AddParentCategory(Page page, int parentId)
    {
        var relatedCategory = pageRepository.GetByIdEager(parentId);
        var previousCachedRelation =
            EntityCache.GetPage(parentId).ChildRelations.LastOrDefault();

        if (previousCachedRelation != null)
        {
            var previousRelation = pageRelationRepo.GetById(previousCachedRelation.Id);
            previousRelation.NextId = page.Id;

            pageRelationRepo.Update(previousRelation);
        }

        var categoryRelationToAdd = new PageRelation()
        {
            Child = page,
            Parent = relatedCategory,
            PreviousId = previousCachedRelation?.ChildId
        };

        pageRelationRepo.Create(categoryRelationToAdd);
    }

    public void AddChild(int parentId, int childId, int authorId)
    {
        var cachedParent = EntityCache.GetPage(parentId);
        var previousCacheRelation = cachedParent?.ChildRelations.LastOrDefault();

        var child = pageRepository.GetById(childId);
        var parent = pageRepository.GetById(parentId);

        var relation = new PageRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = previousCacheRelation?.ChildId,
            NextId = null,
        };

        pageRelationRepo.Create(relation);

        if (previousCacheRelation != null)
        {
            var previousRelation = pageRelationRepo.GetById(previousCacheRelation.Id);
            if (previousRelation != null)
            {
                previousRelation.NextId = childId;
                pageRelationRepo.Update(previousRelation);
            }
        }

        ModifyRelationsEntityCache.AddChild(relation);
        pageRepository.Update(child, authorId, type: PageChangeType.Relations);
        pageRepository.Update(parent, authorId, type: PageChangeType.Relations);
    }

    public int CreateNewRelationAndGetId(int parentId, int childId, int? nextId, int? previousId)
    {
        var child = pageRepository.GetById(childId);
        var parent = pageRepository.GetById(parentId);

        var relation = new PageRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = previousId,
            NextId = nextId,
        };

        pageRelationRepo.Create(relation);
        return relation.Id;
    }

    public void UpdateRelationsInDb(List<PageRelationCache> cachedRelations, int authorId)
    {
        foreach (var r in cachedRelations)
        {
            Logg.r.Information(
                "Job started - ModifyRelations RelationId: {relationId}, Child: {childId}, Parent: {parentId}",
                r.Id, r.ChildId, r.ParentId);

            var relationToUpdate = pageRelationRepo.GetById(r.Id);

            if (relationToUpdate != null)
            {
                var child = pageRepository.GetById(r.ChildId);
                var parent = pageRepository.GetById(r.ParentId);

                relationToUpdate.Child = child;
                relationToUpdate.Parent = parent;
                relationToUpdate.PreviousId = r.PreviousId;
                relationToUpdate.NextId = r.NextId;

                pageRelationRepo.Update(relationToUpdate);

                pageRepository.Update(child, authorId, type: PageChangeType.Relations);
                pageRepository.Update(parent, authorId, type: PageChangeType.Relations);
            }
        }
    }

    public void DeleteRelationInDb(int relationId, int authorId)
    {
        var relationToDelete = relationId > 0 ? pageRelationRepo.GetById(relationId) : null;
        Logg.r.Information("Job started - DeleteRelation RelationId: {relationId}, Child: {childId}, Parent: {parentId}", relationToDelete.Id, relationToDelete.Child.Id, relationToDelete.Parent.Id);

        if (relationToDelete != null)
        {
            pageRelationRepo.Delete(relationToDelete);
            pageRepository.Update(relationToDelete.Child, authorId, type: PageChangeType.Relations);
            pageRepository.Update(relationToDelete.Parent, authorId, type: PageChangeType.Relations);
        }

    }
}
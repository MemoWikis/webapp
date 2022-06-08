using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NHibernate;

public class RelationChangeItem
{
    public bool RelationAdded;
    public bool IsVisibleToCurrentUser;
    public CategoryCacheItem RelatedCategory;
    public CategoryCacheItem[] AffectedParents;

    public static RelationChangeItem GetRelationChangeItem(CategoryChangeDetailModel item, IEnumerable<CategoryChange> changes)
    {
        var selectedRevisionData = CategoryEditData_V2.CreateFromJson(changes.FirstOrDefault(c => c.Id == item.CategoryChangeId).Data);
        var relationChangeItem = new RelationChangeItem();
        var prevRevision = changes.LastOrDefault(c => c.Id < item.CategoryChangeId);

        if (prevRevision == null)
            return null;

        var previousRevisionData = CategoryEditData_V2.CreateFromJson(prevRevision.Data);
        var relationChangeResult = GetChangedRelation(selectedRevisionData, previousRevisionData);

        if (relationChangeResult == null)
            return null;

        relationChangeItem.RelationAdded = relationChangeResult.Count >= 1;

        var relationChange = relationChangeResult.Relation;
        var relatedCategory = EntityCache.GetCategory(relationChange.RelatedCategoryId);
        
        relationChangeItem.IsVisibleToCurrentUser = IsVisibleToCurrentUser2(relationChange, relatedCategory, previousRevisionData, selectedRevisionData, item.CreatorId);
        relationChangeItem.RelatedCategory = relatedCategory;
        relationChangeItem.AffectedParents = item.AffectedParents;

        return relationChangeItem;
    }

    private static bool IsVisibleToCurrentUser2(CategoryRelation_EditData_V2 relationChange, 
        CategoryCacheItem relatedCategory, 
        CategoryEditData_V2 previousRevision, 
        CategoryEditData_V2 selectedRevision, 
        int itemCreatorId)
    {
        var canViewRevisions = PermissionCheck.CanView(UserCache.GetUser(itemCreatorId),
            previousRevision.Visibility,
            selectedRevision.Visibility);

        var relatedCategoryIsVisible = PermissionCheck.CanView(relatedCategory);
        var categoryIsVisible = PermissionCheck.CanViewCategory(relationChange.CategoryId);

        if (categoryIsVisible && relatedCategoryIsVisible && canViewRevisions)
            return true;

        return false;
    }

    private static GetRelationChangeResult GetChangedRelation(CategoryEditData_V2 selectedRevision, CategoryEditData_V2 previousRevision)
    {
        var selectedRevisionCategoryRelations = selectedRevision.CategoryRelations;
        var previousRevisionCategoryRelations = previousRevision.CategoryRelations;

        var selectedRelationsExceptPreviousRelations = selectedRevisionCategoryRelations.Where(l1 => previousRevisionCategoryRelations.All(l2 => l1.RelatedCategoryId != l2.RelatedCategoryId));
        var previousRelationsExceptSelectedRelations = previousRevisionCategoryRelations.Where(l1 => selectedRevisionCategoryRelations.All(l2 => l1.RelatedCategoryId != l2.RelatedCategoryId));

        var noRelationChangeInformation = !selectedRelationsExceptPreviousRelations.Any() && !previousRelationsExceptSelectedRelations.Any();
        var lastSelectedRevisionRelationChange =
            selectedRelationsExceptPreviousRelations.Any() ? selectedRelationsExceptPreviousRelations.Last() : null;
        var lastPreviousRevisionRelationChange =
            previousRelationsExceptSelectedRelations.Any() ? previousRelationsExceptSelectedRelations.Last() : null;

        var getRelationChangeResult = new GetRelationChangeResult();

        if (noRelationChangeInformation || lastSelectedRevisionRelationChange == lastPreviousRevisionRelationChange)
            return null;

        getRelationChangeResult.Count = selectedRevisionCategoryRelations.Count() - previousRevisionCategoryRelations.Count();
        getRelationChangeResult.Relation = selectedRelationsExceptPreviousRelations.Concat(previousRelationsExceptSelectedRelations).Last();

        return getRelationChangeResult;
    }

    public class GetRelationChangeResult
    { 
        public CategoryRelation_EditData_V2 Relation { get; set; }
        public int Count { get; set; }  
    }

    public static string GetRelationChangeLabel(RelationChangeItem item)
    {
        var removalString = item.RelationAdded ? "" : " nicht mehr";


        return $" ist{removalString} verknüpft";
    }
}
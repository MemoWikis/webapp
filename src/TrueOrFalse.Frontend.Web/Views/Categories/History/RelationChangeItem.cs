using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class RelationChangeItem
{
    public bool RelationAdded;
    public CategoryRelationType Type;
    public CategoryCacheItem RelatedCategory;
    public bool IsVisibleToCurrentUser;

    private static CategoryChangeDetailModel _item;
    private static CategoryEditData_V2 _selectedRevision;
    private static CategoryEditData_V2 _previousRevision;
    private static IEnumerable<CategoryRelation_EditData_V2> _selectedRelationsWithoutPreviousRelations;
    private static IEnumerable<CategoryRelation_EditData_V2> _previousRelationsWithoutSelectedRelations;

    public static RelationChangeItem GetRelationChangeItem(CategoryChangeDetailModel item,
        IEnumerable<CategoryChange> changes)
    {
        _item = item;
        GetRevisions(changes);
        var relationChangeItem = new RelationChangeItem();

        if (_previousRevision != null)
        {
            GetRelationChangeDifference(relationChangeItem);

            if (!HasRelationChangeInformation())
                return null;

            BuildRelationChangeItem(relationChangeItem);
        }

        return relationChangeItem;
    }

    private static void GetRelationChangeDifference(RelationChangeItem relationChangeItem)
    {
        var selectedRevisionCategoryRelations = _selectedRevision.CategoryRelations;
        var previousRevisionCategoryRelations = _previousRevision.CategoryRelations;

        _selectedRelationsWithoutPreviousRelations = selectedRevisionCategoryRelations.Where(l1 =>
            previousRevisionCategoryRelations.All(l2 => l1.RelatedCategoryId != l2.RelatedCategoryId));
        _previousRelationsWithoutSelectedRelations = previousRevisionCategoryRelations.Where(l1 =>
            selectedRevisionCategoryRelations.All(l2 => l1.RelatedCategoryId != l2.RelatedCategoryId));

        var count = selectedRevisionCategoryRelations.Count() - previousRevisionCategoryRelations.Count();
        relationChangeItem.RelationAdded = count >= 1;
    }

    private static bool HasRelationChangeInformation()
    {
        var noRelationDifference =
            !_selectedRelationsWithoutPreviousRelations.Any() && !_previousRelationsWithoutSelectedRelations.Any();
        var lastRelationChangeForSelectedRevision =
            _selectedRelationsWithoutPreviousRelations.Any() ? _selectedRelationsWithoutPreviousRelations.Last() : null;
        var lastRelationChangeForPreviousRevision =
            _previousRelationsWithoutSelectedRelations.Any() ? _previousRelationsWithoutSelectedRelations.Last() : null;

        return noRelationDifference ||
               lastRelationChangeForSelectedRevision == lastRelationChangeForPreviousRevision;
    }

    private static void GetRevisions(IEnumerable<CategoryChange> changes)
    {
        _selectedRevision =
            CategoryEditData_V2.CreateFromJson(changes.FirstOrDefault(c => c.Id == _item.CategoryChangeId).Data);
        _previousRevision =
            CategoryEditData_V2.CreateFromJson(changes.LastOrDefault(c => c.Id < _item.CategoryChangeId).Data);
    }

    private static void BuildRelationChangeItem(RelationChangeItem relationChangeItem)
    {
        var relationChange = _selectedRelationsWithoutPreviousRelations
            .Concat(_previousRelationsWithoutSelectedRelations).Last();

        var category = EntityCache.GetCategoryCacheItem(relationChange.CategoryId);
        var relatedCategory = EntityCache.GetCategoryCacheItem(relationChange.RelatedCategoryId);

        var categoryIsVisible = PermissionCheck.CanView(category);
        var relatedCategoryIsVisible = PermissionCheck.CanView(relatedCategory);

        var canViewRevisions = PermissionCheck.CanView(UserCache.GetUser(_item.CreatorId),
            _previousRevision.Visibility,
            _selectedRevision.Visibility);

        if (categoryIsVisible && relatedCategoryIsVisible && canViewRevisions)
            relationChangeItem.IsVisibleToCurrentUser = true;

        relationChangeItem.RelatedCategory = relatedCategory;
        relationChangeItem.Type = relationChange.RelationType;
    }
}
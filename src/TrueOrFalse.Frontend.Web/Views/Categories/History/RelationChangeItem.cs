using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class RelationChangeItem
{
    public bool RelationAdded;
    public bool IsVisibleToCurrentUser;
    public CategoryRelationType Type;
    public CategoryCacheItem RelatedCategory;
    public static RelationChangeItem GetRelationChange(CategoryChangeDetailModel item, IEnumerable<CategoryChange> changes)
    {
        var selectedRevision = CategoryEditData_V2.CreateFromJson(changes.FirstOrDefault(c => c.Id == item.CategoryChangeId).Data);
        var previousRevision = CategoryEditData_V2.CreateFromJson(changes.LastOrDefault(c => c.Id < item.CategoryChangeId).Data);
        var relationChangeItem = new RelationChangeItem();

        if (previousRevision != null)
        {
            var selectedRevisionCategoryRelations = selectedRevision.CategoryRelations;
            var previousRevisionCategoryRelations = previousRevision.CategoryRelations;
            var selectedRevNotPreviousRev = selectedRevisionCategoryRelations.Where(l1 => !previousRevisionCategoryRelations.Any(l2 => l1.RelatedCategoryId == l2.RelatedCategoryId));
            var previousRevNotSelectedRev = previousRevisionCategoryRelations.Where(l1 => !selectedRevisionCategoryRelations.Any(l2 => l1.RelatedCategoryId == l2.RelatedCategoryId));

            var noRelationChangeInformation = !selectedRevNotPreviousRev.Any() && !previousRevNotSelectedRev.Any();
            var lastRelationChangeForSelectedRevision =
                selectedRevNotPreviousRev.Any() ? selectedRevNotPreviousRev.Last() : null;
            var lastRelationChangeForPreviousRevision =
                previousRevNotSelectedRev.Any() ? previousRevNotSelectedRev.Last() : null;

            if (noRelationChangeInformation || lastRelationChangeForSelectedRevision == lastRelationChangeForPreviousRevision)
                return null;

            var count = selectedRevisionCategoryRelations.Count() - previousRevisionCategoryRelations.Count();
            relationChangeItem.RelationAdded = count >= 1;

            var relationChange = selectedRevNotPreviousRev.Concat(previousRevNotSelectedRev).Last();
            var category = EntityCache.GetCategoryCacheItem(relationChange.CategoryId);
            var categoryIsVisible = PermissionCheck.CanView(category);
            var relatedCategory = EntityCache.GetCategoryCacheItem(relationChange.RelatedCategoryId);
            var relatedCategoryIsVisible = PermissionCheck.CanView(relatedCategory);

            var canViewRevisions = PermissionCheck.CanView(UserCache.GetUser(item.CreatorId), 
                previousRevision.Visibility,
                selectedRevision.Visibility);

            if (categoryIsVisible && relatedCategoryIsVisible && canViewRevisions)
                relationChangeItem.IsVisibleToCurrentUser = true;

            relationChangeItem.RelatedCategory = relatedCategory;
            relationChangeItem.Type = relationChange.RelationType;
        }

        return relationChangeItem;
    }

    public static CategoryChangeDetailModel GetSafeItem(CategoryChangeDetailModel item, RelationChangeItem relationChangeItem)
    {
        if (relationChangeItem == null)
        {
            item.Type = CategoryChangeType.Update;
            item.Label = "Update";
            return item;
        }

        item.RelationIsVisibleToCurrentUser = relationChangeItem.IsVisibleToCurrentUser;

        if (relationChangeItem.RelationAdded)
            item.Label += " hinzugefügt";
        else
            item.Label += " entfernt";

        return item;
    }
}
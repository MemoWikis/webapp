using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class CategoryChangesOverviewModel : BaseModel
{
    public IList<CategoryChangeDayModel> Days;
    public int PageToShow;
    public CategoryChangesOverviewModel(int pageToShow)
    {
        PageToShow = pageToShow;
        const int revisionsToShow = 100;
        var revisionsToSkip = (PageToShow - 1) * revisionsToShow;
        var query = $@"SELECT * FROM CategoryChange cc ORDER BY cc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}";
        var revisions = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).List<CategoryChange>();

        
        Days = revisions
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new CategoryChangeDayModel(
                group.Key,
                (IList<CategoryChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }

    public IList<CategoryChange> GetCategoryChanges(CategoryChangeDayModel model)
    {
        var changes = new List<CategoryChange>();

        var currentCategoryIds = model.Items.Select(c => c.CategoryId).Distinct();
        foreach (var id in currentCategoryIds)
            changes.AddRange(Sl.CategoryChangeRepo.GetForCategory(id).OrderBy(c => c.Id));
        return changes;
    }

    public RelationChangeItem GetRelationChange(CategoryChangeDetailModel item, IList<CategoryChange> changes)
    {
        var selectedRevision = CategoryEditData_V2.CreateFromJson(changes.FirstOrDefault(c => c.Id == item.CategoryChangeId).Data);
        var previousRevision = CategoryEditData_V2.CreateFromJson(changes.LastOrDefault(c => c.Id < item.CategoryChangeId).Data);

        var relationChangeItem = new RelationChangeItem();

        if (selectedRevision != null && previousRevision != null)
        {
            var selectedRevisionCategoryRelations = selectedRevision.CategoryRelations;
            var previousRevisionCategoryRelations = previousRevision.CategoryRelations;
            var selectedRevNotPreviousRev = selectedRevisionCategoryRelations.Where(l1 => !previousRevisionCategoryRelations.Any(l2 => l1.RelatedCategoryId == l2.RelatedCategoryId));
            var previousRevNotSelectedRev = previousRevisionCategoryRelations.Where(l1 => !selectedRevisionCategoryRelations.Any(l2 => l1.RelatedCategoryId == l2.RelatedCategoryId));

            var count = selectedRevisionCategoryRelations.Count() - previousRevisionCategoryRelations.Count();
            relationChangeItem.RelationAdded = count >= 1;

            var relationChange = selectedRevNotPreviousRev.Concat(previousRevNotSelectedRev).First();
            var category = EntityCache.GetCategoryCacheItem(relationChange.CategoryId);
            var categoryIsVisible = category != null && PermissionCheck.CanView(category);
            var relatedCategory = EntityCache.GetCategoryCacheItem(relationChange.RelatedCategoryId);
            var relatedCategoryIsVisible = relatedCategory != null && PermissionCheck.CanView(relatedCategory);
            if (!categoryIsVisible || !relatedCategoryIsVisible)
                relationChangeItem.IsVisibleToCurrentUser = false;
            relationChangeItem.RelatedCategory = relatedCategory;
            relationChangeItem.Type = relationChange.RelationType;
        }

        return relationChangeItem;
    }

    public bool IsAuthorOrAdmin(CategoryChangeDetailModel item)
    {
        return Sl.SessionUser.IsInstallationAdmin || Sl.SessionUser.UserId == item.Author.Id;
    }
}
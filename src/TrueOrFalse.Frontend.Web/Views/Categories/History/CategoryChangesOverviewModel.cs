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
        if (item.Type != CategoryChangeType.Relations)
            return null;

        var changesForCurrentCategory = changes.Where(c => c.Category.Id == item.CategoryId);
        return RelationChangeItem.GetRelationChange(item, changesForCurrentCategory);
    }

    public bool IsAuthorOrAdmin(CategoryChangeDetailModel item)
    {
        return _sessionUser.IsInstallationAdmin || _sessionUser.UserId == item.Author.Id;
    }
}
using System;
using System.Linq;
using NHibernate;

public class AllTopicsHistory : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;

    public AllTopicsHistory(PermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }
    public IOrderedEnumerable<IGrouping<DateTime, CategoryChange>> GetGroupedChanges(int page, int revisionsToShow)
    {
        var revisionsToSkip = (page - 1) * revisionsToShow;
        var query =
            $@"SELECT * FROM CategoryChange cc ORDER BY cc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}";
        var orderedTopicChangesOnPage = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange))
            .List<CategoryChange>().OrderBy(c => c.Id);

        var groupedChanges = orderedTopicChangesOnPage
            .Where(ChangeVisibilityCheck)
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => @group.Key);

        return groupedChanges;
    }
    

    private bool ChangeVisibilityCheck(CategoryChange change)
    {
        return change.Category != null && 
               change.Category.Id > 0 &&
               _permissionCheck.CanView(change.Category) &&
               _permissionCheck.CanView(change.Category.Creator.Id, change.GetCategoryChangeData().Visibility);
    }

}


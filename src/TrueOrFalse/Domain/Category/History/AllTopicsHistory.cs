using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Web;

public class AllTopicsHistory : IRegisterAsInstancePerLifetime
{
    public IOrderedEnumerable<IGrouping<DateTime, CategoryChange>> GetDays(int page, int revisionsToShow)
    {
        var revisionsToSkip = (page - 1) * revisionsToShow;
        var query =
            $@"SELECT * FROM CategoryChange cc ORDER BY cc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}";
        var orderedTopicChangesOnPage = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange))
            .List<CategoryChange>().OrderBy(c => c.Id);

        var days = orderedTopicChangesOnPage
            .Where(ChangeVisibilityCheck)
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => @group.Key);

        return days;
    }
    

    private bool ChangeVisibilityCheck(CategoryChange change)
    {
        return change.Category != null && 
               change.Category.Id > 0 &&
               PermissionCheck.CanView(change.Category) &&
               PermissionCheck.CanView(change.Category.Creator.Id, change.GetCategoryChangeData().Visibility);
    }

}


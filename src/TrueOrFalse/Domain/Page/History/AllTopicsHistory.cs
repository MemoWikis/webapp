using NHibernate;

public class AllTopicsHistory(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    ISession _nhibernateSession)
    : IRegisterAsInstancePerLifetime
{
    public IOrderedEnumerable<IGrouping<DateTime, PageChange>> GetGroupedChanges(int page, int revisionsToShow)
    {
        var revisionsToSkip = (page - 1) * revisionsToShow;
        var query = @"SELECT * FROM CategoryChange cc 
            WHERE (cc.Data <> '' AND cc.Data IS NOT NULL AND (JSON_EXTRACT(cc.Data, '$.Visibility') = 0 
            OR (JSON_EXTRACT(cc.Data, '$.Visibility') = 1 AND cc.Author_id = :userId)))
            ORDER BY cc.DateCreated DESC 
            LIMIT :revisionsToSkip, :revisionsToShow";

        var orderedTopicChangesOnPage = _nhibernateSession.CreateSQLQuery(query)
            .AddEntity(typeof(PageChange))
            .SetParameter("userId", _sessionUser.UserId)
            .SetParameter("revisionsToSkip", revisionsToSkip)
            .SetParameter("revisionsToShow", revisionsToShow)
            .List<PageChange>().OrderBy(c => c.Id);

        var groupedChanges = orderedTopicChangesOnPage
            .Where(_permissionCheck.CanView)
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => @group.Key);

        return groupedChanges;
    }
    
}
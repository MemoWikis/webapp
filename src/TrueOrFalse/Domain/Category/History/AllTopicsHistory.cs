using System;
using System.Linq;
using NHibernate;

public class AllTopicsHistory : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;
    private readonly ISession _nhibernateSession;

    public AllTopicsHistory(PermissionCheck permissionCheck,
        SessionUser sessionUser,
        ISession nhibernateSession)
    {
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
        _nhibernateSession = nhibernateSession;
    }
    public IOrderedEnumerable<IGrouping<DateTime, CategoryChange>> GetGroupedChanges(int page, int revisionsToShow)
    {
        var revisionsToSkip = (page - 1) * revisionsToShow;
        var query = $@"SELECT * FROM CategoryChange cc 
            WHERE (cc.Data <> '' AND cc.Data IS NOT NULL AND (JSON_EXTRACT(cc.Data, '$.Visibility') = 0 
            OR (JSON_EXTRACT(cc.Data, '$.Visibility') = 1 AND cc.Author_id = {_sessionUser.UserId})))
            ORDER BY cc.DateCreated DESC 
            LIMIT {revisionsToSkip},{revisionsToShow}";

        var orderedTopicChangesOnPage = _nhibernateSession.CreateSQLQuery(query).AddEntity(typeof(CategoryChange))
            .List<CategoryChange>().OrderBy(c => c.Id);

        var groupedChanges = orderedTopicChangesOnPage
            .Where(_permissionCheck.CanView)
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => @group.Key);

        return groupedChanges;
    }
    
}


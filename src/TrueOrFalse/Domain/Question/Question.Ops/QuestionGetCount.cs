using System;
using NHibernate;
using NHibernate.Criterion;

public class QuestionGetCount : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public QuestionGetCount(ISession session){
        _session = session;
    }

    public int Run(){
        return (int)_session.CreateQuery(
            "SELECT Count(Id) " +
            "FROM Question " +
            "WHERE Visibility = 0 " +
            "AND IsWorkInProgress = 0").UniqueResult<Int64>();
    }

    public int Run(int creatorId)
    {
        return _session.QueryOver<Question>()
            .Where(s => 
                s.Creator.Id == creatorId && 
                s.IsWorkInProgress == false)
            .Select(Projections.RowCount())
            .FutureValue<int>()
            .Value;
    }

    public int Run(int creatorId, int categoryId, QuestionVisibility[] visibility)
    {
        return _session.QueryOver<Question>()
            .Where(q =>
                q.Creator.Id == creatorId &&
                q.IsWorkInProgress == false)
                .AndRestrictionOn(q => q.Visibility).IsIn(visibility)
            .JoinQueryOver<int>(q => q.CategoriesIds)
            .Where(Id => Id == categoryId)
            .Select(Projections.RowCount())
            .FutureValue<int>()
            .Value;
    }
}
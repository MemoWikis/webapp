﻿using NHibernate;

public class ActivityPointsRepo : RepositoryDb<ActivityPoints>
{
    public ActivityPointsRepo(ISession session) : base(session)
    {
    }

    public IList<ActivityPoints> GetActivtyPointsByUser(int userId)
    {
        return Session.QueryOver<ActivityPoints>()
            .Where(x => x.UserId == userId)
            .List();
    }
}
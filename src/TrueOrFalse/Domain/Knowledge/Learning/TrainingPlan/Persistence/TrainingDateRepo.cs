using System;
using System.Collections.Generic;
using NHibernate;

public class TrainingDateRepo : RepositoryDbBase<TrainingDate>
{
    public TrainingDateRepo(ISession session) : base(session)
    {
    }

    public IList<TrainingDate> AllPastNotNotified()
    {
        return Session
            .QueryOver<TrainingDate>()
            .Where(d =>
                d.DateTime < DateTime.Now &&
                d.NotificationStatus == NotificationStatus.None
            ).List();
    } 
}
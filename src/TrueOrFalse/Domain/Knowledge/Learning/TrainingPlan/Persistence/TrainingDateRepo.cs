using System;
using System.Collections.Generic;
using NHibernate;

public class TrainingDateRepo : RepositoryDbBase<TrainingDate>
{
    public TrainingDateRepo(ISession session) : base(session)
    {
    }

    public IList<TrainingDate> AllDue_InLessThen7Minutes_NotNotified()
    {   
        return Session
            .QueryOver<TrainingDate>()
            .Where(d =>
                d.DateTime < DateTime.Now.AddMinutes(7) &&
                d.NotificationStatus == NotificationStatus.None
            ).List();
    } 
}
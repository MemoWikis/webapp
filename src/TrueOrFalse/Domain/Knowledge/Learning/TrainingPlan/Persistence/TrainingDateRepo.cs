using System;
using System.Collections.Generic;
using NHibernate;

public class TrainingDateRepo : RepositoryDbBase<TrainingDate>
{
    public TrainingDateRepo(ISession session) : base(session)
    {
    }

    public IList<TrainingDate> AllDue_InLessThen7Minutes_NotNotified(int maxAgeMinutes = 30)
    {   
        return Session
            .QueryOver<TrainingDate>()
            .Where(d =>
                d.DateTime < DateTime.Now.AddMinutes(7) &&
                d.DateTime > DateTime.Now.AddMinutes(-maxAgeMinutes) &&
                d.NotificationStatus == NotificationStatus.None
            ).List();
    }

    public TrainingDate GetByLearningSessionId(int learningSessionId)
    {
        return Session
            .QueryOver<TrainingDate>()
            .Where(d => d.LearningSession != null && d.LearningSession.Id == learningSessionId)
            .SingleOrDefault();
    }
}
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

    public IList<TrainingDate> GetUpcomingTrainingDates(int withinDays = 7)
    {
        var currentUser = _userSession.User;
        return _session
            .QueryOver<TrainingDate>()
            .OrderBy(d => d.DateTime).Asc
            .Where(d => d.DateTime < DateTime.Now.AddDays(withinDays) && d.DateTime > DateTime.Now)
            .JoinQueryOver<TrainingPlan>(x => x.TrainingPlan)
            .JoinQueryOver<Date>(y => y.Date)
            .Where(d => d.User == currentUser)
            .List<TrainingDate>();
        // Equals about: Select * from trainingdate as td LEFT JOIN trainingplan as tp ON td.TrainingPlan_Id = tp.Id LEFT JOIN date as d ON tp.Date_id = d.Id WHERE d.User_id = currentUser
    }
}
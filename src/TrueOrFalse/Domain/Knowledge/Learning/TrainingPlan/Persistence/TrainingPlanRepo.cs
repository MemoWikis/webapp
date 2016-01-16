using System;
using NHibernate;

public class TrainingPlanRepo : RepositoryDbBase<TrainingPlan>
{
    public TrainingPlanRepo(ISession session) : base(session)
    {
    }

    public void DeleteDates(int trainingPlanId, DateTime after)
    {
        Sl.Session.CreateSQLQuery(
            "DELETE FROM trainingplan " +
            "WHERE  Id = " + trainingPlanId + " " +
            "AND datecreated > '" + after.ToString("yyy-MM-dd HH:mm:ss") + "'" )
            .ExecuteUpdate();

    }
}
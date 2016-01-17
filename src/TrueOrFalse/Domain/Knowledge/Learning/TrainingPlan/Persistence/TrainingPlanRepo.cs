using System;
using System.Linq;
using NHibernate;
using NHibernate.Util;

public class TrainingPlanRepo : RepositoryDbBase<TrainingPlan>
{
    public TrainingPlanRepo(ISession session) : base(session)
    {
    }

    public void DeleteDates(TrainingPlan trainingPlan, DateTime after)
    {
        trainingPlan.Dates = trainingPlan.Dates.Where(x => x.DateTime <= after).ToList();

        Sl.Session.CreateSQLQuery(
            "DELETE FROM trainingdate " +
            "WHERE  TrainingPlan_Id = " + trainingPlan.Id + " " +
            "AND datetime > '" + after.ToString("yyy-MM-dd HH:mm:ss") + "'" )
            .ExecuteUpdate();
    }

    public override void Create(TrainingPlan trainingPlan)
    {
        trainingPlan.Dates.ForEach(x =>{
            x.DateCreated = DateTime.Now;
            x.DateModified = DateTime.Now;
        });

        base.Create(trainingPlan);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Util;

public class TrainingPlanRepo : RepositoryDbBase<TrainingPlan>
{
    public TrainingPlanRepo(ISession session) : base(session)
    {
    }

    public void DeleteUnstartedDatesAfter(TrainingPlan trainingPlan, DateTime after)
    {
        var datesToDelete = trainingPlan.Dates
            .Where(d => d.LearningSession == null && d.DateTime > after)
            .ToList();

        datesToDelete.ForEach(d => trainingPlan.Dates.Remove(d));

        Update(trainingPlan);
        Flush();
    }

    public override void Update(TrainingPlan trainingPlan)
    {
        trainingPlan.Dates.ForEach(x =>
        {
            if (x.DateCreated == DateTime.MinValue){
                x.DateCreated = DateTime.Now;
                x.DateModified = DateTime.Now;
            }
        });

        base.Update(trainingPlan);
    }

    public override void Create(TrainingPlan trainingPlan)
    {
        trainingPlan.Dates.ForEach(x => {
            x.DateCreated = DateTime.Now;
            x.DateModified = DateTime.Now;
        });

        base.Create(trainingPlan);
    }

    public IList<TrainingPlan> AllWithNewMissedDates()
    {
        //return PastDates.Any(d => d.LearningSession == null && !d.MarkedAsMissed);

        var newMissedDates = Session
            .QueryOver<TrainingDate>()
            .Where(d =>
                d.ExpiresAt <= DateTimeX.Now() 
                && d.LearningSession == null
                && !d.MarkedAsMissed
            ).List();

        return newMissedDates.Select(d => d.TrainingPlan).Distinct().ToList();
    }

    public IList<TrainingPlan> AllWithExpiredUncompletedDates()
    {
        var uncompletedDates = Session
            .QueryOver<TrainingDate>()
            .Where(d =>
                d.ExpiresAt <= DateTimeX.Now()
                && d.LearningSession != null
            )
            .JoinQueryOver(d => d.LearningSession)
            .Where(l => !l.IsCompleted)
            .List();

        return uncompletedDates.Where(d => d.IsExpired()).Select(d => d.TrainingPlan).Distinct().ToList();
    }
}
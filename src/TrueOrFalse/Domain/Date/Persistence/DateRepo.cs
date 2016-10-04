using System;
using System.Collections.Generic;
using System.Diagnostics;
using NHibernate;
using NHibernate.Criterion;

public class DateRepo : RepositoryDbBase<Date>
{
    public DateRepo(ISession session) : base(session)
    {
    }

    public void CreateWithTrainingPlan(Date date)
    {
        Create(date);
        var trainingPlan = TrainingPlanCreator.Run(date, TrainingPlanSettings.GetSettingsWithIndividualDefaults(date));
        date.TrainingPlan = trainingPlan;

        var stopWatch = Stopwatch.StartNew();
        Sl.R<TrainingPlanRepo>().Create(trainingPlan);
        Logg.r().Information("Traingplan created (in db): {duration}", stopWatch.Elapsed);

        Update(date);
        Logg.r().Information("Traingplan added to date (in db): {duration}", stopWatch.Elapsed);
    }

    public void UpdateWithTrainingPlan(Date date)
    {
        TrainingPlanUpdater.Run(date.TrainingPlan);
        Update(date);
        Flush();
    }

    public override void Create(Date date)
    {
        base.Create(date);
        Flush();
        UserActivityAdd.CreatedDate(date);
        ReputationUpdate.ForUser(date.User);
    }

    public int Copy(Date sourceDate)
    {
        var sets = new List<Set>();
        sets.AddRange(sourceDate.Sets);

        Date copiedDate = new Date
        {
            User = _userSession.User,
            Sets = sets,
            Details = sourceDate.Details,
            DateTime = sourceDate.DateTime,
            Visibility = sourceDate.Visibility,
            CopiedFrom = sourceDate
        };

        CreateWithTrainingPlan(copiedDate);
        ReputationUpdate.ForUser(sourceDate.User);
        return copiedDate.Id;
    }

    public IList<Date> GetBy(int[] userIds = null, bool onlyUpcoming = false, bool onlyPrevious = false, bool onlyVisibleToNetwork = false)
    {
        var queryOver = _session.QueryOver<Date>();

        if(userIds != null)
            queryOver = queryOver.WhereRestrictionOn(u => u.User.Id).IsIn(userIds);

        if (onlyVisibleToNetwork)
            queryOver.Where(d => d.Visibility == DateVisibility.InNetwork);

        if (onlyUpcoming)
            queryOver.Where(d => d.DateTime >= DateTime.Now);

        if (onlyPrevious)
            queryOver.Where(d => d.DateTime < DateTime.Now);

        queryOver = queryOver.OrderBy(q => q.DateTime).Asc;

        return queryOver.List();
    }

    public IList<Date> GetBy(int userId, bool onlyUpcoming = false, bool onlyPrevious = false, bool onlyVisibleToNetwork = false)
    {
        return GetBy(new [] {userId}, onlyUpcoming, onlyPrevious, onlyVisibleToNetwork);
    }

    public IList<Date> GetBySet(int setId)
    {
        return _session.QueryOver<Date>()
            .JoinQueryOver<Set>(d => d.Sets)
            .Where(s => s.Id == setId)
            .List<Date>();
    }

    public int AmountOfPreviousItems(int userId)
    {
        return _session.QueryOver<Date>()
            .Where(d => d.User.Id == userId)
            .And(d => d.DateTime < DateTime.Now)
            .Select(Projections.RowCount())
            .SingleOrDefault<int>();
    }
}
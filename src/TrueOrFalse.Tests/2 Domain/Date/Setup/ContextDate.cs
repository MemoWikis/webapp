
using System;
using System.Collections.Generic;

public class ContextDate
{
    public List<Date> All = new List<Date>();
    
    private readonly DateRepo _dateRepo;

    public User User;

    private ContextDate()
    {
        _dateRepo = Sl.R<DateRepo>();
        User = ContextUser.New().Add("First Name").Persist().All[0];
    }

    public static ContextDate New()
    {
        return new ContextDate();
    }

    public ContextDate Add(IList<Set> sets, User creator = null, DateTime dateTime = default(DateTime))
    {
        var date = new Date();
        date.Details = "Details";
        date.Sets = sets;
        date.User = creator ?? User;
        date.DateTime = dateTime;
        date.TrainingPlan = new TrainingPlan();

        All.Add(date);
        return this;
    }

    public ContextDate Persist()
    {
        foreach (var date in All)
            _dateRepo.Create(date);

        return this;
    }
}
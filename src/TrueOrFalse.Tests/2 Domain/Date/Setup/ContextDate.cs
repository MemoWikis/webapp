
using System.Collections.Generic;

public class ContextDate : IRegisterAsInstancePerLifetime
{
    public List<Date> All = new List<Date>();
    
    private readonly DateRepo _dateRepo;

    public User User;

    public ContextDate()
    {
        _dateRepo = Sl.R<DateRepo>();
        User = ContextUser.New().Add("First Name").Persist().All[0];
    }

    public static ContextDate New()
    {
        return BaseTest.Resolve<ContextDate>();
    }

    public ContextDate Add(IList<Set> sets, User creator = null)
    {
        var date = new Date();
        date.Details = "Details";
        date.Sets = sets;
        date.User = creator ?? User;

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
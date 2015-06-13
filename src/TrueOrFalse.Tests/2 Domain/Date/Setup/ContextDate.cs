
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

    public ContextDate Add(IList<Set> sets)
    {
        var date = new Date();
        date.Details = "Details";
        date.Sets = sets;
        date.User = User;

        All.Add(date);
        return this;
    }

    public ContextDate Persist()
    {
        foreach (var cat in All)
            _dateRepo.Create(cat);

        return this;
    }
}
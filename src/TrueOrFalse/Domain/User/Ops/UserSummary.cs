using NHibernate;
using NHibernate.Criterion;

public class UserSummary : IRegisterAsInstancePerLifetime
{
    public int AmountCreatedQuestions(int creatorId)
    {
        return Sl.Resolve<ISession>()
            .QueryOver<Question>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator.Id == creatorId)
            .FutureValue<int>().Value;
    }

    public int AmountCreatedSets(int creatorId)
    {
        return Sl.Resolve<ISession>()
            .QueryOver<Set>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator.Id == creatorId)
            .FutureValue<int>().Value;            
    }

    public int AmountCreatedCategories(int creatorId)
    {
        return Sl.Resolve<ISession>()
            .QueryOver<Category>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator.Id == creatorId)
            .FutureValue<int>().Value;
    }

}
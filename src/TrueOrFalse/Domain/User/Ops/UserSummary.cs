using NHibernate;
using NHibernate.Criterion;

public class UserSummary : IRegisterAsInstancePerLifetime
{
    public int AmountCreatedQuestions(int creatorId, bool inclPrivateQuestions = true)
    {
        var query = Sl.Resolve<ISession>()
            .QueryOver<Question>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator.Id == creatorId);

        if (!inclPrivateQuestions)
            query = query.Where(q => q.Visibility == QuestionVisibility.All);

        return query.FutureValue<int>().Value;
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
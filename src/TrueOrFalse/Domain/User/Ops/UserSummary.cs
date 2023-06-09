﻿using NHibernate;
using NHibernate.Criterion;

public class UserSummary : IRegisterAsInstancePerLifetime
{
    public int AmountCreatedQuestions(int creatorId, bool inclPrivateQuestions = true)
    {
        var query = Sl.Resolve<ISession>()
            .QueryOver<Question>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator != null && q.Creator.Id == creatorId);

        if (!inclPrivateQuestions)
            query = query.Where(q => q.Visibility == QuestionVisibility.All);

        return query.FutureValue<int>().Value;
    }

    public int AmountCreatedCategories(int creatorId, bool inclPrivateCategories = true)
    {
        var query = Sl.Resolve<ISession>()
            .QueryOver<Category>()
            .Select(Projections.RowCount())
            .Where(c => c.Creator != null && c.Creator.Id == creatorId);

        if (!inclPrivateCategories)
            query = query.Where(q => q.Visibility == CategoryVisibility.All);

        return query.FutureValue<int>().Value;
    }

}
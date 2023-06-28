using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;

public class CategoryViewRepo : RepositoryDb<CategoryView>
{
    public CategoryViewRepo(ISession session) : base(session) { }

    public int GetViewCount(int categoryId)
    {
        return _session.QueryOver<CategoryView>()
            .Select(Projections.RowCount())
            .Where(x => x.Category.Id == categoryId)
            .FutureValue<int>()
            .Value;
    }
}
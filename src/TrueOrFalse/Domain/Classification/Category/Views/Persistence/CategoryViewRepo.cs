using NHibernate;
using NHibernate.Criterion;
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

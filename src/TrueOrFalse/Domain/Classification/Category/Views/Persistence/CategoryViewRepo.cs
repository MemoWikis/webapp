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

    public IList<ViewsPerDay> GetPerDay(int categoryId)
    {
        return _session.CreateSQLQuery($@"
                SELECT 
                    count(DATE(DateCreated)) Views, 
                    DATE(DateCreated) Date 
                FROM categoryview 
                WHERE Category_id = {categoryId}
                GROUP BY Date")
            .SetResultTransformer(Transformers.AliasToBean(typeof(ViewsPerDay)))
            .List<ViewsPerDay>();
    }
}
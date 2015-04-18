using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class UpdateSetCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public UpdateSetCountForCategory(ISession session)
    {
        _session = session;
    }

    public void Run(IList<Category> categoryIds)
    {
        Run(categoryIds.Select(c => c.Id));
    }

    public void Run(IEnumerable<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            var query =
                "UPDATE category SET CountSets = " +
                "(SELECT COUNT(*) FROM categories_to_sets WHERE Category_id = category.Id )" +
                "WHERE Id = " + categoryId;

            _session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}
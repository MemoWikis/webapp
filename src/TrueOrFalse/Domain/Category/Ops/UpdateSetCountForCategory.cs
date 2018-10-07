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

    public void Run(IList<Category> categories)
    {
        Run(categories.Select(c => c.Id));
    }

    public void Run(IEnumerable<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            Sl.CategoryRepo.GetById(categoryId).CountSets = Sl.SetRepo.GetForCategory(categoryId).Count;
        }
    }

    public void RunWithSql(IList<Category> categories)
    {
        RunWithSql(categories.Select(c => c.Id));
    }

    public void RunWithSql(IEnumerable<int> categoryIds)
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
     
    public void RunAggregated(IEnumerable<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            var query =
                $@"
                    UPDATE category SET CountSets = 
                    (
                        SELECT COUNT(DISTINCT cs.Set_id) FROM categories_to_sets cs
                        WHERE cs.Category_id = { categoryId}

                        UNION

                        SELECT COUNT(DISTINCT cs.Set_id) FROM relatedcategoriestorelatedcategories rc
                        INNER JOIN category c
                        ON rc.Related_Id = c.Id
                        INNER JOIN categories_to_sets cs
                        ON c.Id = cs.Category_id
                        WHERE rc.Category_id = { categoryId}
                        AND rc.CategoryRelationType = { (int)CategoryRelationType.IncludesContentOf}
                    )
                    WHERE Id = {categoryId}";

            _session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}
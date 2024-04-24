using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;

public class CategoryValuationReadingRepo(ISession _session) : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<CategoryValuation> _repo = new(_session);

    public CategoryValuation GetBy(int categoryId, int userId) =>
        _repo.Session.QueryOver<CategoryValuation>()
            .Where(q => q.UserId == userId && q.CategoryId == categoryId)
            .SingleOrDefault();

    public IList<CategoryValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _repo.Session.QueryOver<CategoryValuation>()
            .Where(q => q.UserId == userId);

        if (onlyActiveKnowledge)
            query.Where(q => q.RelevancePersonal >= -1);

        return query.List<CategoryValuation>();
    }

    public IList<CategoryValuation> GetByCategory(int categoryId) =>
        _repo.Session.QueryOver<CategoryValuation>()
            .Where(q =>
                q.CategoryId == categoryId &&
                q.RelevancePersonal >= 0)
            .List<CategoryValuation>();

    public IList<CategoryValuation> GetBy(IList<int> categoryIds, int userId)
    {
        if (!categoryIds.Any())
            return new List<CategoryValuation>();

        var sb = new StringBuilder();
        sb.Append("SELECT * FROM CategoryValuation WHERE UserId = " + userId + " ");
        sb.Append("AND (CategoryId = " + categoryIds[0]);

        for (int i = 1; i < categoryIds.Count; i++)
            sb.Append(" OR CategoryId = " + categoryIds[i]);

        sb.Append(")");

        return _repo.Session.CreateSQLQuery(sb.ToString())
            .SetResultTransformer(Transformers.AliasToBean(typeof(CategoryValuation)))
            .List<CategoryValuation>();
    }
}
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistence;

public class PageValuationReadingRepository(ISession _session) : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<PageValuation> _repo = new(_session);

    public PageValuation GetBy(int categoryId, int userId) =>
        _repo.Session.QueryOver<PageValuation>()
            .Where(q => q.UserId == userId && q.PageId == categoryId)
            .SingleOrDefault();

    public IList<PageValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _repo.Session.QueryOver<PageValuation>()
            .Where(q => q.UserId == userId);

        if (onlyActiveKnowledge)
            query.Where(q => q.RelevancePersonal >= -1);

        return query.List<PageValuation>();
    }

    public IList<PageValuation> GetByPage(int categoryId) =>
        _repo.Session.QueryOver<PageValuation>()
            .Where(q =>
                q.PageId == categoryId &&
                q.RelevancePersonal >= 0)
            .List<PageValuation>();
}
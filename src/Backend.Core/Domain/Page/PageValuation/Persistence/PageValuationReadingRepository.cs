using NHibernate;


public class PageValuationReadingRepository(ISession _session) : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<PageValuation> _repo = new(_session);

    public PageValuation GetBy(int pageId, int userId)
    {
        Log.Debug("PageValuationReadingRepository.GetBy: pageId={PageId}, userId={UserId}", pageId, userId);

        var result = _repo.Session.QueryOver<PageValuation>()
            .Where(q => q.UserId == userId && q.PageId == pageId)
            .SingleOrDefault();

        Log.Debug("PageValuationReadingRepository.GetBy: result found={Found}", result != null);

        if (result != null)
        {
            Log.Debug(
                "PageValuationReadingRepository.GetBy: result details - UserId={UserId}, PageId={PageId}, RelevancePersonal={RelevancePersonal}",
                result.UserId, result.PageId, result.RelevancePersonal);
        }

        return result;
    }

    public IList<PageValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        Log.Debug(
            "PageValuationReadingRepository.GetByUser: userId={UserId}, onlyActiveKnowledge={OnlyActiveKnowledge}",
            userId, onlyActiveKnowledge);

        var query = _repo.Session.QueryOver<PageValuation>()
            .Where(q => q.UserId == userId);

        if (onlyActiveKnowledge)
            query.Where(q => q.RelevancePersonal >= -1);

        var results = query.List<PageValuation>();

        Log.Debug("PageValuationReadingRepository.GetByUser: found {Count} page valuations for userId {UserId}",
            results.Count, userId);

        foreach (var pv in results.Take(5)) // Log first 5 for debugging
        {
            Log.Debug(
                "PageValuationReadingRepository.GetByUser: PageValuation - UserId: {UserId}, PageId: {PageId}, RelevancePersonal: {RelevancePersonal}",
                pv.UserId, pv.PageId, pv.RelevancePersonal);
        }

        return results;
    }

    public IList<PageValuation> GetByPage(int pageId) =>
        _repo.Session.QueryOver<PageValuation>()
            .Where(q =>
                q.PageId == pageId &&
                q.RelevancePersonal >= 0)
            .List<PageValuation>();
}
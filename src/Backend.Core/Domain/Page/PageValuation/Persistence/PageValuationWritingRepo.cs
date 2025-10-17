using NHibernate;


public class PageValuationWritingRepo(ISession _session, KnowledgeSummaryLoader _knowledgeSummaryLoader)
    : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<PageValuation> _repo = new(_session);

    private void UpdateKnowledgeSummary(PageValuation pageValuation)
    {
        var knowledgeSummary = _knowledgeSummaryLoader.Run(pageValuation.UserId, pageValuation.PageId, false);
        // Combined results from InWishknowledge and NotInWishknowledge
        pageValuation.CountNotLearned = knowledgeSummary.InWishknowledge.NotLearned + knowledgeSummary.NotInWishknowledge.NotLearned;
        pageValuation.CountNeedsLearning = knowledgeSummary.InWishknowledge.NeedsLearning + knowledgeSummary.NotInWishknowledge.NeedsLearning;
        pageValuation.CountNeedsConsolidation = knowledgeSummary.InWishknowledge.NeedsConsolidation + knowledgeSummary.NotInWishknowledge.NeedsConsolidation;
        pageValuation.CountSolid = knowledgeSummary.InWishknowledge.Solid + knowledgeSummary.NotInWishknowledge.Solid;
    }

    public void Update(PageValuation pageValuation)
    {
        UpdateKnowledgeSummary(pageValuation);
        _repo.Update(pageValuation);
    }

    public void DeletePageValuation(int pageId)
    {
        _repo.Session.CreateSQLQuery("DELETE FROM pagevaluation WHERE PageId = :pageId")
            .SetParameter("pageId", pageId).ExecuteUpdate();
    }
}
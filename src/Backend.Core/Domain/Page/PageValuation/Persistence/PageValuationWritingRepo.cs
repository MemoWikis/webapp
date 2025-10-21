using NHibernate;


public class PageValuationWritingRepo(ISession _session, KnowledgeSummaryLoader _knowledgeSummaryLoader)
    : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<PageValuation> _repo = new(_session);

    private void UpdateKnowledgeSummary(PageValuation pageValuation)
    {
        var knowledgeSummary = _knowledgeSummaryLoader.Run(pageValuation.UserId, pageValuation.PageId, false);
        // Combined results from InWishKnowledge and NotInWishKnowledge
        pageValuation.CountNotLearned = knowledgeSummary.InWishKnowledge.NotLearned + knowledgeSummary.NotInWishKnowledge.NotLearned;
        pageValuation.CountNeedsLearning = knowledgeSummary.InWishKnowledge.NeedsLearning + knowledgeSummary.NotInWishKnowledge.NeedsLearning;
        pageValuation.CountNeedsConsolidation = knowledgeSummary.InWishKnowledge.NeedsConsolidation + knowledgeSummary.NotInWishKnowledge.NeedsConsolidation;
        pageValuation.CountSolid = knowledgeSummary.InWishKnowledge.Solid + knowledgeSummary.NotInWishKnowledge.Solid;
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
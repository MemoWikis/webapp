﻿using NHibernate;


public class PageValuationWritingRepo(ISession _session, KnowledgeSummaryLoader _knowledgeSummaryLoader)
    : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<PageValuation> _repo = new(_session);

    private void UpdateKnowledgeSummary(PageValuation pageValuation)
    {
        var knowledgeSummary = _knowledgeSummaryLoader.Run(pageValuation.UserId, pageValuation.PageId, false);
        pageValuation.CountNotLearned = knowledgeSummary.NotLearned;
        pageValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        pageValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        pageValuation.CountSolid = knowledgeSummary.Solid;
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
using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryValuationWritingRepo(ISession _session, KnowledgeSummaryLoader _knowledgeSummaryLoader)
    : IRegisterAsInstancePerLifetime
{
    private readonly RepositoryDb<CategoryValuation> _repo = new(_session);

    private void UpdateKnowledgeSummary(CategoryValuation categoryValuation)
    {
        var c = categoryValuation; 
        var knowledgeSummary = _knowledgeSummaryLoader.Run(c.UserId, c.CategoryId, false);
        c.CountNotLearned = knowledgeSummary.NotLearned;
        c.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        c.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        c.CountSolid = knowledgeSummary.Solid;
    }

    public void Update(CategoryValuation categoryValuation)
    {
        UpdateKnowledgeSummary(categoryValuation);
        _repo.Update(categoryValuation);
    }

    public void DeleteCategoryValuation( int categoryId)
    {
        _repo.Session.CreateSQLQuery("DELETE FROM categoryvaluation WHERE CategoryId = :categoryId")
            .SetParameter("categoryId", categoryId).ExecuteUpdate();
    }
}
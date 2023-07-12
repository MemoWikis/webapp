using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;

public class CategoryValuationRepo : RepositoryDb<CategoryValuation>
{
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;

    public CategoryValuationRepo(ISession session, KnowledgeSummaryLoader knowledgeSummaryLoader) : base(session)
    {
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
    }

    public IList<CategoryValuation> GetBy(int categoryId) =>
        _session.QueryOver<CategoryValuation>()
                .Where(s => s.CategoryId == categoryId)
                .List<CategoryValuation>();

    public CategoryValuation GetBy(int categoryId, int userId) => 
        _session.QueryOver<CategoryValuation>()
                .Where(q => q.UserId == userId && q.CategoryId == categoryId)
                .SingleOrDefault();

    public IList<CategoryValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _session.QueryOver<CategoryValuation>()
            .Where(q => q.UserId == userId);

        if (onlyActiveKnowledge)
            query.Where(q => q.RelevancePersonal >= -1);
            
        return query.List<CategoryValuation>();
    }

    private void UpdateKnowledgeSummary(CategoryValuation categoryValuation)
    {
        var c = categoryValuation; 
        var knowledgeSummary = _knowledgeSummaryLoader.Run(c.UserId, c.CategoryId, false);
        c.CountNotLearned = knowledgeSummary.NotLearned;
        c.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        c.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        c.CountSolid = knowledgeSummary.Solid;
    }

    public IList<CategoryValuation> GetByCategory(int categoryId) =>
        _session.QueryOver<CategoryValuation>()
            .Where(q =>
                q.CategoryId == categoryId &&
                q.RelevancePersonal >= 0)
            .List<CategoryValuation>();

    public bool IsInWishKnowledge(int categoryId, int userId) =>
        GetBy(categoryId, userId)?.IsInWishKnowledge() ?? false;

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

        return _session.CreateSQLQuery(sb.ToString())
                        .SetResultTransformer(Transformers.AliasToBean(typeof(CategoryValuation)))
                        .List<CategoryValuation>();
    }

    public override void Update(CategoryValuation categoryValuation)
    {
        UpdateKnowledgeSummary(categoryValuation);
        base.Update(categoryValuation);
    }

    public void DeleteCategoryValuation( int categoryId)
    {
        Session.CreateSQLQuery("DELETE FROM categoryvaluation WHERE CategoryId = :categoryId")
            .SetParameter("categoryId", categoryId).ExecuteUpdate();
    }
}
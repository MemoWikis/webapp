using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;

public class CategoryValuationRepo : RepositoryDb<CategoryValuation>
{
    public CategoryValuationRepo(ISession session) : base(session)
    {
    }

    public IList<CategoryValuation> GetBy(int categoryId) =>
        _session.QueryOver<CategoryValuation>()
                .Where(s => s.CategoryId == categoryId)
                .List<CategoryValuation>();

    public CategoryValuation GetBy(int categoryId, int userId) => 
        _session.QueryOver<CategoryValuation>()
                .Where(q => q.UserId == userId && q.CategoryId == categoryId)
                .SingleOrDefault();

    public IList<CategoryValuation> GetByUser(int userId) => 
        _session.QueryOver<CategoryValuation>()
                .Where(q =>
                    q.UserId == userId &&
                    q.RelevancePersonal >= 0)
                .List<CategoryValuation>();

    public IList<CategoryValuation> GetByCategory(int categoryId) =>
        _session.QueryOver<CategoryValuation>()
            .Where(q =>
                q.CategoryId == categoryId &&
                q.RelevancePersonal >= 0)
            .List<CategoryValuation>();

    public bool IsInWishKnowledge(int categoryId, int userId) =>
        Sl.CategoryValuationRepo.GetBy(categoryId, userId)?.IsInWishKnowledge() ?? false;

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

    public void UpdateKnowledgeSummariesForUser(int userId)
    {
        foreach (var categoryValuation in GetByUser(userId))
        {
            UpdateKnowledgeSummary(categoryValuation);
        }
    }

    public void UpdateKnowledgeSummariesForCategory(int catgoryId)
    {
        foreach (var categoryValuation in GetByCategory(catgoryId))
        {
            UpdateKnowledgeSummary(categoryValuation);
        }
    }

    public void UpdateKnowledgeSummary(CategoryValuation categoryValuation)
    {
        categoryValuation.UpdateKnowledgeSummary();

        Update(categoryValuation);
    }

    public override void Create(IList<CategoryValuation> valuations)
    {
        base.Create(valuations);
        var categories = Sl.CategoryRepo.GetByIds(valuations.GetCategoryIds().ToArray());
        Sl.SearchIndexCategory.Update(categories);
    }

    public override void Create(CategoryValuation categoryValuation)
    {
        categoryValuation.UpdateKnowledgeSummary();
        base.Create(categoryValuation);
        Sl.SearchIndexCategory.Update(Sl.CategoryRepo.GetById(categoryValuation.CategoryId));
    }

    public override void CreateOrUpdate(CategoryValuation categoryValuation)
    {
        categoryValuation.UpdateKnowledgeSummary();
        base.CreateOrUpdate(categoryValuation);
        Sl.SearchIndexCategory.Update(Sl.CategoryRepo.GetById(categoryValuation.CategoryId));
    }

    public override void Update(CategoryValuation categoryValuation)
    {
        categoryValuation.UpdateKnowledgeSummary();
        base.Update(categoryValuation);
        Sl.SearchIndexCategory.Update(Sl.CategoryRepo.GetById(categoryValuation.CategoryId));
    }
}
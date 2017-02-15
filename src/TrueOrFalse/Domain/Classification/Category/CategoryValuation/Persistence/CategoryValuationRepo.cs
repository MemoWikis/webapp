using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryValuationRepo : RepositoryDb<CategoryValuation>
{
    public CategoryValuationRepo(ISession session) : base(session)
    {
    }

    public IList<CategoryValuation> GetByUser(int userId)
    {
        return _session.QueryOver<CategoryValuation>()
                        .Where(q =>
                            q.UserId == userId &&
                            q.RelevancePersonal >= 0)
                        .List<CategoryValuation>();
    }

    public override void Create(IList<CategoryValuation> valuations)
    {
        base.Create(valuations);
        var categories = Sl.CategoryRepo.GetByIds(valuations.GetCategoryIds().ToArray());
        Sl.SearchIndexCategory.Update(categories);
    }

    public override void Create(CategoryValuation categoryValuation)
    {
        base.Create(categoryValuation);
        Sl.SearchIndexCategory.Update(Sl.CategoryRepo.GetById(categoryValuation.CategoryId));
    }

    public override void CreateOrUpdate(CategoryValuation categoryValuation)
    {
        base.CreateOrUpdate(categoryValuation);
        Sl.SearchIndexCategory.Update(Sl.CategoryRepo.GetById(categoryValuation.CategoryId));
    }

    public override void Update(CategoryValuation categoryValuation)
    {
        base.Update(categoryValuation);
        Sl.SearchIndexCategory.Update(Sl.CategoryRepo.GetById(categoryValuation.CategoryId));
    }
}
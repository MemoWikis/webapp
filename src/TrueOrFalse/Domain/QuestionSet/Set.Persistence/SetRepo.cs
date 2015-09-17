using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using TrueOrFalse.Search;

public class SetRepo : RepositoryDbBase<Set>
{
    private readonly SearchIndexSet _searchIndexSet;

    public SetRepo(
        ISession session, 
        SearchIndexSet searchIndexSet)
        : base(session)
    {
        _searchIndexSet = searchIndexSet;
    }

    public override void Update(Set set)
    {
        _searchIndexSet.Update(set);
        base.Update(set);

        var categoriesToUpdate =
            _session.CreateSQLQuery("SELECT Category_id FROM categories_to_sets WHERE Set_id =" + set.Id)
            .List<int>().ToList();

        categoriesToUpdate.AddRange(set.Categories.Select(x => x.Id).ToList());
        categoriesToUpdate = categoriesToUpdate.GroupBy(x => x).Select(x => x.First()).ToList();
        Sl.Resolve<UpdateSetCountForCategory>().Run(categoriesToUpdate);
    }

    public override void Create(Set set)
    {
        base.Create(set);
        Sl.Resolve<UpdateSetCountForCategory>().Run(set.Categories);
        _searchIndexSet.Update(set);
    }

    public IList<Set> GetByIds(List<int> setIds)
    {
        return GetByIds(setIds.ToArray());
    }

    public override IList<Set> GetByIds(params int[] setIds)
    {
        var resultTmp = base.GetByIds(setIds);
        var result = new List<Set>();
        for (int i = 0; i < setIds.Length; i++)
        {
            if (resultTmp.Any(c => c.Id == setIds[i]))
                result.Add(resultTmp.First(c => c.Id == setIds[i]));
        }
        return result;
    }

    public IList<Set>GetForCategory(int categoryId)
    {
        return _session.QueryOver<Set>()
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Set>();
    }

    public IEnumerable<Set> GetMostRecent(int amount)
    {
        return _session
            .QueryOver<Set>()
            .OrderBy(s => s.DateCreated)
            .Desc
            .Take(amount)
            .List();
    }

    public IEnumerable<TopSetResult> GetMostQuestions(int amount)
    {
        return _session.CreateSQLQuery("SELECT QCount, Set_id as SetId, Name, Text FROM " +
                                       "(SELECT count(questioninset.Question_id) AS QCount, questioninset.Set_id " +
                                       "FROM questioninset GROUP BY Set_id ORDER BY QCount DESC " +
                                       "LIMIT "+ amount +") AS qis_r " +
                                       "LEFT JOIN questionset ON qis_r.Set_id = questionset.Id")
                        .SetResultTransformer(Transformers.AliasToBean(typeof(TopSetResult)))
                        .List<TopSetResult>().ToList();
    }


    public override void Delete(Set set)
    {
        if (Sl.R<SessionUser>().IsLoggedInUserOrAdmin(set.Creator.Id))
            throw new InvalidAccessException();

        base.Delete(set);
        Flush();
    }

}
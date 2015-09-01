using System.Collections.Generic;
using System.Linq;
using NHibernate;
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
        ThrowIfNot_IsUserOrAdmin(set.Id);

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

    //public IEnumerable<Set> GetMostQuestions(int amount)
    //{
    //    return _session
    //        .QueryOver<Set>()
    //        .OrderBy(s => s.QuestionsInSet().Count).Desc
    //        .Take(amount)
    //        .List();
    //}

    public override void Delete(Set set)
    {
        ThrowIfNot_IsUserOrAdmin(set.Id);

        base.Delete(set);
        Flush();
    }

}
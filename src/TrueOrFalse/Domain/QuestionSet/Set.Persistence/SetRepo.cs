using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
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

    public IList<Set> GetByCreatorId(int creatorId)
    {
        return _session.QueryOver<Set>()
            .Where(c => c.Creator.Id == creatorId)
            .List<Set>();
    }

    public IList<Set> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Set>()
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Set>();
    }

    public Set GetByIdEager(int setId)
    {
        //Question creatorAlias = null;

        return _session.QueryOver<Set>()
            .Where(set => set.Id == setId)
            .Left.JoinQueryOver<QuestionInSet>(s => s.QuestionsInSet)
            .Left.JoinQueryOver(s => s.Question)
            .Left.JoinQueryOver<Category>(s => s.Categories)
            .SingleOrDefault();
    }

    public IList<Set> GetByIdsEager(int[] setIds = null)
    {
        var query = _session.QueryOver<Set>();

        if (setIds != null)
            query = query.Where(Restrictions.In("Id", setIds));

        return query.Left.JoinQueryOver<QuestionInSet>(s => s.QuestionsInSet)
            .Left.JoinQueryOver(s => s.Question)
            .Left.JoinQueryOver<Category>(s => s.Categories)
            .List()
            .GroupBy(s => s.Id)
            .Select(s => s.First())
            .ToList();
    }

    public IList<Set> GetAllEager() => GetByIdsEager();


    public IEnumerable<Set> GetMostRecent_WithAtLeast3Questions(int amount)
    {
        string query = $@"SELECT s.* FROM questionset s
            LEFT JOIN questioninset qs
            ON s.Id = qs.Set_Id 
            GROUP BY s.Id
            Having Count(qs.Set_Id )  > 3
            ORDER BY s.DateCreated DESC
            LIMIT {amount}";

        return _session
            .CreateSQLQuery(query)
            .AddEntity(typeof(Set))
            .List<Set>();
    }

    public IEnumerable<TopSetResult> GetMostQuestions(int amount)
    {
        return _session.CreateSQLQuery(
            "SELECT QCount, Set_id as SetId, Name, Text FROM " +
            "(SELECT count(questioninset.Question_id) AS QCount, questioninset.Set_id " +
            "FROM questioninset GROUP BY Set_id ORDER BY QCount DESC " +
            "LIMIT "+ amount +") AS qis_r " +
            "LEFT JOIN questionset ON qis_r.Set_id = questionset.Id")
            .SetResultTransformer(Transformers.AliasToBean(typeof(TopSetResult)))
            .List<TopSetResult>().ToList();
    }

    /// <summary>
    /// Return how often a set is in other peoples WuWi
    /// </summary>
    public int HowOftenInOtherPeoplesWuwi(int userId, int setId)
    {
        return Sl.R<SetValuationRepo>()
            .Query
            .Where(v =>
                v.UserId != userId &&
                v.SetId == setId &&
                v.RelevancePersonal > -1
            )
            .RowCount();
    }

    public override void Delete(Set set)
    {
        //if (Sl.R<SessionUser>().IsLoggedInUserOrAdmin(set.Creator.Id))
        //    throw new InvalidAccessException(); //exception is thrown, even if admin (=creator of set) is logged in

        _searchIndexSet.Delete(set);
        base.Delete(set);
        foreach (var category in set.Categories)
        {
            category.UpdateCountQuestionsAggregated();
        }
        Flush();
    }

    public int HowManyNewSetsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Set>()
            .Where(s => s.DateCreated > since)
            .RowCount();
    }

    public int TotalSetCount()
    {
        return _session.QueryOver<Set>()
            .RowCount();
    }

    public string GetYoutbeUrl(int setId) => 
        _session.Query<Set>()
            .Where(s => s.Id == setId)
            .Select(s => s.VideoUrl)
            .FirstOrDefault();
}
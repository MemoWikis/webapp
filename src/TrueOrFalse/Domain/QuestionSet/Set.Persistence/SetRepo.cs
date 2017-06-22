﻿using System;
using System.Collections.Generic;
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

    public override void Update(Set set)
    {
        _searchIndexSet.Update(set);
        base.Update(set);

        var categoriesToUpdateIds =
            _session.CreateSQLQuery("SELECT Category_id FROM categories_to_sets WHERE Set_id =" + set.Id)
            .List<int>().ToList();

        Flush();//Flush exactly here: If category has been added, categories_to_sets entry has been added already (important for UpdateSetCountForCategory). If category has been removed, it is still included in categoriesToUpdate.

        categoriesToUpdateIds.AddRange(set.Categories.Select(x => x.Id).ToList());
        categoriesToUpdateIds = categoriesToUpdateIds.GroupBy(x => x).Select(x => x.First()).ToList();
        Sl.Resolve<UpdateSetCountForCategory>().Run(categoriesToUpdateIds);

        Sl.Resolve<UpdateSetDataForQuestion>().Run(set.QuestionsInSet);

        var aggregatedCategoriesToUpdate =
            CategoryAggregation.GetInterrelatedCategories(Sl.CategoryRepo.GetByIds(categoriesToUpdateIds));

        foreach (var category in aggregatedCategoriesToUpdate)
        {
            category.UpdateAggregatedSets();
        }
    }

    public override void Create(Set set)
    {
        base.Create(set);
        Flush();
        Sl.Resolve<UpdateSetCountForCategory>().Run(set.Categories);
        UserActivityAdd.CreatedSet(set);
        ReputationUpdate.ForUser(set.Creator);
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
            //.Fetch(s => s.Creator).Eager
            //.Left.JoinAlias(s => s.Creator, () => creatorAlias)
            .Left.JoinQueryOver<QuestionInSet>(s => s.QuestionsInSet)
            .Left.JoinQueryOver(s => s.Question)
            .Left.JoinQueryOver<Category>(s => s.Categories)
            .SingleOrDefault();
    }

    public override IList<Set> GetAll()
    {
        return _session.QueryOver<Set>()
            .Left.JoinQueryOver<QuestionInSet>(s => s.QuestionsInSet)
            .Left.JoinQueryOver(s => s.Question)
            .Left.JoinQueryOver<Category>(s => s.Categories)
            .List();
    }

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

    /// <summary>
    /// Return how often a set is part of a future or previous date
    /// </summary>
    public int HowOftenInDate(int setId)
    {
        return _session.QueryOver<Date>()
            .JoinQueryOver<Set>(d => d.Sets)
            .Where(s => s.Id == setId)
            .Select(Projections.RowCount())
            .SingleOrDefault<int>();
    }

    public override void Delete(Set set)
    {
        //if (Sl.R<SessionUser>().IsLoggedInUserOrAdmin(set.Creator.Id))
        //    throw new InvalidAccessException(); //exception is thrown, even if admin (=creator of set) is logged in

        _searchIndexSet.Delete(set);
        base.Delete(set);
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
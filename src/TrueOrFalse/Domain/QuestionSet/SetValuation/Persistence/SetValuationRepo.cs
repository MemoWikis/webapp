﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

public class SetValuationRepo : RepositoryDb<SetValuation> 
{
    private readonly SearchIndexSet _searchIndexSet;
    private readonly SetRepo _setRepo;

    public SetValuationRepo(
        ISession session, 
        SearchIndexSet searchIndexSet,
        SetRepo setRepo) : base(session)
    {
        _searchIndexSet = searchIndexSet;
        _setRepo = setRepo;
    }

    public IList<SetValuation> GetBy(int setId)
    {
        return _session.QueryOver<SetValuation>()
                        .Where(s => s.SetId == setId).List<SetValuation>();
    }

    public SetValuation GetBy(int setId, int userId)
    {
        return _session.QueryOver<SetValuation>()
                        .Where(q => q.UserId == userId && q.SetId == setId)
                        .SingleOrDefault();
    }

    public IList<SetValuation> GetByUser(int userId)
    {
        return _session.QueryOver<SetValuation>()
                        .Where(q =>
                            q.UserId == userId &&
                            q.RelevancePersonal >= 0)
                        .List<SetValuation>();
    }

    public IList<SetValuation> GetBy(IList<int> setIds, int userId)
    {
        if (!setIds.Any())
            return new List<SetValuation>();

        var sb = new StringBuilder();
        sb.Append("SELECT * FROM SetValuation WHERE UserId = " + userId + " ");
        sb.Append("AND (SetId = " + setIds[0]);

        for(int i = 1; i < setIds.Count(); i++){
            sb.Append(" OR SetId = " + setIds[i]);
        }
        sb.Append(")");

        return _session.CreateSQLQuery(sb.ToString())
                        .SetResultTransformer(Transformers.AliasToBean(typeof(SetValuation)))
                        .List<SetValuation>();
    }

    public override void Create(IList<SetValuation> setValuations)
    {
        base.Create(setValuations);
        _searchIndexSet.Update(_setRepo.GetByIds(setValuations.SetIds().ToArray()));
    }

    public override void Create(SetValuation setValuation)
    {
        base.Create(setValuation);
        _searchIndexSet.Update(_setRepo.GetById(setValuation.SetId));
    }

    public override void CreateOrUpdate(SetValuation setValuation)
    {
        base.CreateOrUpdate(setValuation);
        _searchIndexSet.Update(_setRepo.GetById(setValuation.SetId));
    }

    public override void Update(SetValuation setValuation)
    {
        base.Update(setValuation);
        _searchIndexSet.Update(_setRepo.GetById(setValuation.SetId));
    }

    public void DeleteWhereSetIdIs(int setId)
    {
        var setValuations = GetBy(setId);
        var userIds = setValuations.Select(x => x.UserId).Distinct().ToArray();
        var users = Sl.Resolve<UserRepo>().GetByIds(userIds);

        _session.Delete("FROM SetValuation WHERE SetId = " + setId + "");

        Sl.Resolve<UpdateWishcount>().Run(users);
    }
}
using System;
using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class SetsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly SetRepository _setRepo;
    private readonly SearchSets _searchSets;

    public SetsControllerSearch(
        SetRepository setRepo, 
        SearchSets searchSets)
    {
        _setRepo = setRepo;
        _searchSets = searchSets;
    }

    public IList<Set> Run(SetSearchSpec searchSpecSetMine)
    {
        var solrResult = _searchSets.Run(
            searchSpecSetMine.SearchTearm,
            searchSpecSetMine,
            searchSpecSetMine.Filter.CreatorId.IsActive() ? 
                Convert.ToInt32(searchSpecSetMine.Filter.CreatorId.GetValue()) : 
                -1);

        return _setRepo.GetByIds(solrResult.SetIds.ToArray());
    }
}
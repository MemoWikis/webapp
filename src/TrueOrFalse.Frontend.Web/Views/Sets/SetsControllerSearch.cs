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

    public IList<Set> Run(SetSearchSpec searchSpecSet)
    {
        var solrResult = _searchSets.Run(
            searchSpecSet.SearchTerm,
            searchSpecSet,
            searchSpecSet.Filter.CreatorId.IsActive() ? 
                Convert.ToInt32(searchSpecSet.Filter.CreatorId.GetValue()) : 
                -1, 
            searchSpecSet.Filter.ValuatorId);

        return _setRepo.GetByIds(solrResult.SetIds.ToArray());
    }
}
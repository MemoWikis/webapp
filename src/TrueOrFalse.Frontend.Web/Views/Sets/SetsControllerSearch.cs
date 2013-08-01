using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class SetsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly SetRepository _setRepo;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchSets _searchSets;

    public SetsControllerSearch(
        SetRepository setRepo, 
        SessionUiData sessionUiData, 
        SearchSets searchSets)
    {
        _setRepo = setRepo;
        _sessionUiData = sessionUiData;
        _searchSets = searchSets;
    }

    public IList<Set> Run()
    {
        if (string.IsNullOrEmpty(_sessionUiData.SearchSpecSet.SearchTearm))
            return SearchFromSqlServer();
        
        return SearchFromSOLR();
    }

    private IList<Set> SearchFromSOLR()
    {
        var solrResult = _searchSets.Run(
            _sessionUiData.SearchSpecSet.SearchTearm,
            _sessionUiData.SearchSpecSet);

        return _setRepo.GetByIds(solrResult.SetIds.ToArray());
    }

    private IList<Set> SearchFromSqlServer()
    {
        return _setRepo.GetBy(_sessionUiData.SearchSpecSet);
    }
}
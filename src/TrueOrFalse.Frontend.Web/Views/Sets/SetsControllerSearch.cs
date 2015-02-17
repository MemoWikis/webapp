using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Search;

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
        var solrResult = _searchSets.Run(searchSpecSet);
        return _setRepo.GetByIds(solrResult.SetIds.ToArray());
    }
}
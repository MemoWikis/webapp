using System.Collections.Generic;
using TrueOrFalse.Search;

public class SetsControllerSearch
{
    public IList<Set> Run(SetSearchSpec searchSpecSet)
    {
        var solrResult = Sl.R<SearchSets>().Run(searchSpecSet);
        return Sl.R<SetRepo>().GetByIds(solrResult.SetIds.ToArray());
    }
}
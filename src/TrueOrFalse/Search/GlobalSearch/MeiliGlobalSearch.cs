using System.Threading.Tasks;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
public class MeiliGlobalSearch : IGlobalSearch
{
    public async Task<SolrGlobalSearchResult> Go(string term, string type)
    {
        var result = new SolrGlobalSearchResult();
        var pageSize = 5;
        result.CategoriesResult = await new MeiliSearchCategories().RunAsync(term);
        
        
        return result;
    }


    public SolrGlobalSearchResult GoAllCategories(string term, int[] categoriesToFilter = null)
    {
        var pager = new Pager {QueryAll = true};
        var result = new SolrGlobalSearchResult
        {
            CategoriesResult = Sl.SearchCategories.Run(term, pager, categoriesToFilter: categoriesToFilter)
        };
        return result;
    }
}
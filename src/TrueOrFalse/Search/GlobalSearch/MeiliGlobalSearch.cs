using System.Threading.Tasks;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
public class MeiliGlobalSearch : IGlobalSearch
{
    public async Task<GlobalSearchResult> Go(string term, string type)
    {
        var result = new GlobalSearchResult();
        var pageSize = 5;
        result.CategoriesResult = await new MeiliSearchCategories().RunAsync(term, new Pager { PageSize = pageSize });
        
        
        return result;
    }


    public GlobalSearchResult GoAllCategories(string term, int[] categoriesToFilter = null)
    {
        var pager = new Pager {QueryAll = true};
        var result = new GlobalSearchResult
        {
            CategoriesResult = Sl.SearchCategories.Run(term, pager, categoriesToFilter: categoriesToFilter)
        };
        return result;
    }
}
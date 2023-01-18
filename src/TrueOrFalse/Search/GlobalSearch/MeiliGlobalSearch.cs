using Seedworks.Lib.Persistence;

using TrueOrFalse.Search;
public class MeiliGlobalSearch : IGlobalSearch
{
    public GlobalSearchResult Go(string term, string type)
    {
        var result = new GlobalSearchResult();

        var pageSize = 5;
        result.CategoriesResult =
           Sl.MeiliSearchCategories.Run(term, new Pager { PageSize = pageSize }).Result;
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
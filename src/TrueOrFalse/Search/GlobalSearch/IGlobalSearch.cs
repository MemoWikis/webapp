using System.Threading.Tasks;
using TrueOrFalse.Search;

    public interface IGlobalSearch
    {
        Task<SolrGlobalSearchResult> Go(string term, string type);
        SolrGlobalSearchResult GoAllCategories(string term, int[] categoriesToFilter = null);
    }

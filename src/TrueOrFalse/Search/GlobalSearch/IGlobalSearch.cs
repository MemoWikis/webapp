using System.Threading.Tasks;
using TrueOrFalse.Search;

public interface IGlobalSearch
{
    Task<GlobalSearchResult> Go(string term, string type);
    Task<GlobalSearchResult> GoAllCategories(string term, int[] categoriesToFilter = null);
}

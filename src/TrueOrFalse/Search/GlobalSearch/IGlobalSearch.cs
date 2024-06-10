using TrueOrFalse.Search;

public interface IGlobalSearch
{
    Task<GlobalSearchResult> Go(string term);

    Task<GlobalSearchResult> GoAllCategoriesAsync(string term);

    Task<GlobalSearchResult> GoNumberOfCategories(string term, int size = 5);
}
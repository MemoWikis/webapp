using TrueOrFalse.Search;

public interface IGlobalSearch
{
    Task<GlobalSearchResult> Go(string term);

    Task<GlobalSearchResult> GoAllPagesAsync(string term);

    Task<GlobalSearchResult> GoNumberOfPages(string term, int size = 5);
}
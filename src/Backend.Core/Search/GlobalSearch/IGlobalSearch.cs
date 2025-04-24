using TrueOrFalse.Search;

public interface IGlobalSearch
{
    Task<GlobalSearchResult> Go(string term, List<Language> languages);

    Task<GlobalSearchResult> GoAllPagesAsync(string term);

    Task<GlobalSearchResult> GoNumberOfPages(string term, int size = 5);
}
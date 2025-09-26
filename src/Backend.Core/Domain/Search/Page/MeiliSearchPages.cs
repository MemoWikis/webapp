using Meilisearch;

public class MeilisearchPages(PermissionCheck _permissionCheck, int _size = 5)
    : MeilisearchBase, IRegisterAsInstancePerLifetime
{
    private List<PageCacheItem> _pages = new();
    private string _currentUserName = string.Empty;

    public MeilisearchPages(PermissionCheck permissionCheck, int size, string currentUserName) 
        : this(permissionCheck, size)
    {
        _currentUserName = currentUserName;
    }

    public async Task<ISearchPagesResult> RunAsync(string searchTerm, List<Language>? languages = null)
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        var index = client.Index(MeilisearchIndices.Pages);

        return await LoadSearchResults(searchTerm, index, languages);
    }

    private async Task<MeilisearchPagesResult> LoadSearchResults(
        string searchTerm,
        Meilisearch.Index index,
        List<Language>? languages = null
    )
    {
        var finalResults = new List<MeilisearchPageMap>();

        // Search for pages in specified languages first (if provided)
        await SearchPagesInSpecifiedLanguage(searchTerm, index, languages, finalResults);

        // Then search for all other pages
        var allResults = await SearchPagesInAllLanguages(searchTerm, index);

        // Add results that aren't already in the list
        MergeNonDuplicateResults(finalResults, allResults);

        // Sort results to prioritize user's own pages first (if user is logged in)
        if (!string.IsNullOrEmpty(_currentUserName))
        {
            finalResults = finalResults
                .OrderByDescending(result => result.CreatorName == _currentUserName)
                .ThenBy(result => result.Id)
                .ToList();
        }

        FilterCacheItems(finalResults);

        if (IsReloadRequired(finalResults.Count, _pages.Count()))
        {
            _count += 20;
            await LoadSearchResults(searchTerm, index, languages);
        }

        var pageIds = _pages
            .Select(page => page.Id)
            .Take(_size)
            .ToList();

        var result = new MeilisearchPagesResult();
        result.PageIds.AddRange(pageIds);
        result.Count = finalResults.Count;

        return result;
    }

    private async Task SearchPagesInSpecifiedLanguage(string searchTerm, Meilisearch.Index index, List<Language>? languages,
        List<MeilisearchPageMap> finalResults)
    {
        if (languages != null && languages.Any())
        {
            var clauses = languages
                .Select(lang => lang.GetCode())
                .Select(code => $"Language = \"{code}\"")
                .ToList();

            var sqBoosted = new SearchQuery
            {
                Q = searchTerm,
                Limit = _count,
                Filter = string.Join(" OR ", clauses)
            };
            var resBoosted = await index.SearchAsync<MeilisearchPageMap>(searchTerm, sqBoosted);
            finalResults.AddRange(resBoosted.Hits);
        }
    }

    private async Task<ISearchable<MeilisearchPageMap>> SearchPagesInAllLanguages(string searchTerm, Meilisearch.Index index)
    {
        var searchQuery = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };
        var allResults = await index.SearchAsync<MeilisearchPageMap>(searchTerm, searchQuery);
        return allResults;
    }

    private static void MergeNonDuplicateResults(List<MeilisearchPageMap> finalResults, ISearchable<MeilisearchPageMap> allResults)
    {
        var existingIds = finalResults.Select(result => result.Id).ToHashSet();
        var additionalResults = allResults.Hits.Where(result => !existingIds.Contains(result.Id));
        finalResults.AddRange(additionalResults);
    }

    private void FilterCacheItems(List<MeilisearchPageMap> pageMaps)
    {
        var pagesTemp = EntityCache
            .GetPages(pageMaps.Select(page => page.Id))
            .Where(_permissionCheck.CanView)
            .ToList();

        _pages.AddRange(pagesTemp);
        _pages = _pages.Distinct().ToList();
    }
}
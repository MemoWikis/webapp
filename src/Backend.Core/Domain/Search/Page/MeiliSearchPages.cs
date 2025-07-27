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
        var allResults = new List<MeilisearchPageMap>();

        // Search for pages in specified languages first (if provided)
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
            allResults.AddRange(resBoosted.Hits);
        }

        // Then search for all other pages
        var searchQuery = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };
        var resAll = await index.SearchAsync<MeilisearchPageMap>(searchTerm, searchQuery);
        
        // Add results that aren't already in the list
        var existingIds = allResults.Select(r => r.Id).ToHashSet();
        var additionalResults = resAll.Hits.Where(r => !existingIds.Contains(r.Id));
        allResults.AddRange(additionalResults);

        // Sort results to prioritize user's own pages first (if user is logged in)
        if (!string.IsNullOrEmpty(_currentUserName))
        {
            allResults = allResults
                .OrderByDescending(r => r.CreatorName == _currentUserName)
                .ThenBy(r => r.Id)
                .ToList();
        }

        FilterCacheItems(allResults);

        if (IsReloadRequired(allResults.Count, _pages.Count()))
        {
            _count += 20;
            await LoadSearchResults(searchTerm, index, languages);
        }

        var pageIds = _pages
            .Select(c => c.Id)
            .Take(_size)
            .ToList();

        var result = new MeilisearchPagesResult();
        result.PageIds.AddRange(pageIds);
        result.Count = allResults.Count;

        return result;
    }

    private void FilterCacheItems(List<MeilisearchPageMap> pageMaps)
    {
        var pagesTemp = EntityCache
            .GetPages(pageMaps.Select(c => c.Id))
            .Where(_permissionCheck.CanView)
            .ToList();

        _pages.AddRange(pagesTemp);
        _pages = _pages.Distinct().ToList();
    }
}
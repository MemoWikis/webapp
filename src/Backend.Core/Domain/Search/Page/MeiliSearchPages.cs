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

        // Search for pages in specified languages (if provided)
        var languageResults = await SearchPagesInSpecifiedLanguage(searchTerm, index, languages);

        // Search for all other pages
        var allResults = await SearchPagesInAllLanguages(searchTerm, index);

        // Merge results while preserving MeiliSearch relevance order
        MergeResultsPreservingRelevance(finalResults, languageResults, allResults);

        // Sort results to prioritize user's own pages first while preserving MeiliSearch order within each group
        if (!string.IsNullOrEmpty(_currentUserName))
        {
            var userResults = finalResults
                .Where(result => result.CreatorName == _currentUserName)
                .ToList();
            var otherResults = finalResults
                .Where(result => result.CreatorName != _currentUserName)
                .ToList();
            
            finalResults = userResults.Concat(otherResults).ToList();
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

    private async Task<ISearchable<MeilisearchPageMap>?> SearchPagesInSpecifiedLanguage(string searchTerm, Meilisearch.Index index, List<Language>? languages)
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
                Limit = _count * 2, // Get more results to avoid drowning out good matches
                Filter = string.Join(" OR ", clauses)
            };
            var resBoosted = await index.SearchAsync<MeilisearchPageMap>(searchTerm, sqBoosted);
            return resBoosted;
        }
        return null;
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

    private static void MergeResultsPreservingRelevance(List<MeilisearchPageMap> finalResults, ISearchable<MeilisearchPageMap>? languageResults, ISearchable<MeilisearchPageMap> allResults)
    {
        var seenIds = new HashSet<int>();
        
        // Add language-specific results first (if any), preserving their MeiliSearch order
        if (languageResults != null)
        {
            foreach (var result in languageResults.Hits)
            {
                if (seenIds.Add(result.Id))
                {
                    finalResults.Add(result);
                }
            }
        }
        
        // Add remaining results from all languages search, preserving MeiliSearch order
        foreach (var result in allResults.Hits)
        {
            if (seenIds.Add(result.Id))
            {
                finalResults.Add(result);
            }
        }
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
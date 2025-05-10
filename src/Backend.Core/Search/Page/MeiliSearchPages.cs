using Meilisearch;

public class MeilisearchPages(PermissionCheck _permissionCheck, int _size = 5)
    : MeilisearchBase, IRegisterAsInstancePerLifetime
{
    private List<PageCacheItem> _pages = new();

    public async Task<ISearchPagesResult> RunAsync(string searchTerm, List<Language>? languages = null)
    {
        var client = new MeilisearchClient(Settings.MeiliSearchUrl, Settings.MeiliSearchMasterKey);
        var index = client.Index(MeilisearchIndices.Pages);

        return await LoadSearchResults(searchTerm, index, languages);
    }

    private async Task<MeilisearchPagesResult> LoadSearchResults(
        string searchTerm,
        Meilisearch.Index index,
        List<Language>? languages = null
    )
    {
        var hits = new List<MeilisearchPageMap>();
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
            hits = resBoosted.Hits.ToList();
        }

        var searchQuery = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };
        var resAll = await index.SearchAsync<MeilisearchPageMap>(searchTerm, searchQuery);
        var allMaps = resAll.Hits.ToList();

        var remainder = allMaps.Where(a => hits.All(b => b.Id != a.Id));
        var results = hits.Concat(remainder).ToList();

        FilterCacheItems(results);

        if (IsReloadRequired(results.Count, _pages.Count()))
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
        result.Count = results.Count;

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
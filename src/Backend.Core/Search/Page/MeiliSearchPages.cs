using Meilisearch;

namespace TrueOrFalse.Search;

public class MeiliSearchPages : MeiliSearchHelper, IRegisterAsInstancePerLifetime
{
    private List<PageCacheItem> _pages = new();
    private MeiliSearchPagesResult _result;
    private readonly PermissionCheck _permissionCheck;
    private int _size;

    public MeiliSearchPages(PermissionCheck permissionCheck, int size = 5)
    {
        _permissionCheck = permissionCheck;
        _size = size;
    }

    public async Task<ISearchPagesResult> RunAsync(string searchTerm, List<Language>? languages = null)
    {
        var client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
        var index = client.Index(MeiliSearchConstants.Pages);
        _result = new MeiliSearchPagesResult();

        _result.PageIds.AddRange(
            await LoadSearchResults(searchTerm, index, languages)
        );

        return _result;
    }

    private async Task<List<int>> LoadSearchResults(
        string searchTerm,
        Meilisearch.Index index,
        List<Language>? languages = null
    )
    {
        var boostedMaps = new List<MeiliSearchPageMap>();
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
            var resBoosted = await index.SearchAsync<MeiliSearchPageMap>(searchTerm, sqBoosted);
            boostedMaps = resBoosted.Hits.ToList();
        }

        var sqAll = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };
        var resAll = await index.SearchAsync<MeiliSearchPageMap>(searchTerm, sqAll);
        var allMaps = resAll.Hits.ToList();

        var remainder = allMaps.Where(a => boostedMaps.All(b => b.Id != a.Id));
        var merged = boostedMaps.Concat(remainder).ToList();

        _result.Count = merged.Count;

        FilterCacheItems(merged);

        if (IsReloadRequired(merged.Count, _pages.Count()))
        {
            _count += 20;
            await LoadSearchResults(searchTerm, index, languages);
        }

        return _pages
            .Select(c => c.Id)
            .Take(_size)
            .ToList();
    }

    private void FilterCacheItems(List<MeiliSearchPageMap> pageMaps)
    {
        var pagesTemp = EntityCache
            .GetPages(pageMaps.Select(c => c.Id))
            .Where(_permissionCheck.CanView)
            .ToList();

        _pages.AddRange(pagesTemp);
        _pages = _pages.Distinct().ToList();
    }
}


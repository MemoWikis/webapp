using Meilisearch;

public class MeilisearchUsers : MeilisearchBase, IRegisterAsInstancePerLifetime
{
    private List<UserCacheItem> _users = new();

    private MeilisearchUsersResult _result = new();

    public async Task<ISearchUsersResult> RunAsync(string searchTerm, List<Language>? languages = null)
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        var index = client.Index(MeilisearchIndices.Users);

        _result.UserIds.AddRange(await LoadSearchResults(searchTerm, index, languages));

        return _result;
    }

    private async Task<List<int>> LoadSearchResults(string searchTerm, Meilisearch.Index index, List<Language>? languages = null)
    {
        var sq = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };

        if (languages != null && languages.Any())
        {
            var clauses = languages
                .Select(lang => lang.GetCode())
                .Select(code => $"ContentLanguages = \"{code}\"")
                .ToList();

            sq.Filter = string.Join(" OR ", clauses);
        }

        var userMaps =
            (await index.SearchAsync<MeiliSearchUserMap>(searchTerm, sq))
            .Hits;

        var userMapsSkip = userMaps
            .Skip(_count - 20) //skip 0
            .ToList();

        AddUsers(userMapsSkip);

        if (IsReloadRequired(userMaps.Count, _users.Count()))
        {
            _count += 20;
            await LoadSearchResults(searchTerm, index, languages);
        }

        ;
        _result.Count = _users.Count;
        return _users
            .Select(c => c.Id)
            .Take(5)
            .ToList();
    }

    private void AddUsers(List<MeiliSearchUserMap> userMaps)
    {
        var ids = userMaps.Select(c => c.Id);
        var usersTemp = EntityCache.GetUsersByIds(ids)
            .ToList();
        _users.AddRange(usersTemp);
        _users = _users
            .Distinct()
            .ToList();
    }

    public async Task<(List<MeiliSearchUserMap> searchResultUser, Pager pager)>
        GetUsersByPagerAsync(string searchTerm, Pager pager, SearchUsersOrderBy orderBy, string[] languageCodes)
    {
        var userMaps = new List<MeiliSearchUserMap>();
        var count = 0;
        var languages = LanguageExtensions.GetLanguages(languageCodes);
        if (string.IsNullOrEmpty(searchTerm))
        {
            userMaps = EntityCache.GetAllUsers().Where(u => LanguageExtensions.AnyLanguageIsInList(languages, u.ContentLanguages)).Select(ConvertToUserMap).ToList();
            count = userMaps.Count;
        }
        else
        {
            var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
            var index = client.Index(MeilisearchIndices.Users);

            string filterString = null;

            if (languages.Any())
            {
                var clauses = languages
                    .Select(lang => lang.GetCode()) // "German", "English", etc.
                    .Select(code => $"ContentLanguages = \"{code}\"")
                    .ToList();

                filterString = string.Join(" OR ", clauses);
            }

            // Create the search query
            var sq = new SearchQuery
            {
                // The search text
                Q = searchTerm,
                // Limit how many hits we fetch
                Limit = 100,
                // Add the filter (only if we have at least one language selected)
                Filter = filterString
            };

            var searchResult = await index.SearchAsync<MeiliSearchUserMap>(searchTerm, sq);
            if (searchResult is SearchResult<MeiliSearchUserMap> result)
            {
                count = result.EstimatedTotalHits;
            }
            else
            {
                Log.Error("fail cast from ISearchable to SearchResult");
            }
            userMaps = searchResult.Hits.ToList();
        }

        var userMapsOrdered = new List<MeiliSearchUserMap>();
        switch (orderBy)
        {
            case SearchUsersOrderBy.None:
                userMapsOrdered = userMaps.OrderBy(um => um.Id).ToList();
                break;
            case SearchUsersOrderBy.Rank:
                userMapsOrdered = userMaps.OrderBy(um => um.Rank).ToList();
                break;
            case SearchUsersOrderBy.WishCount:
                userMapsOrdered = userMaps.OrderBy(um => um.WishCountQuestions).ToList();
                break;
        }

        if (string.IsNullOrEmpty(searchTerm))
        {
            userMapsOrdered = userMapsOrdered
                .Skip(pager.LowerBound - 1)
                .Take(pager.PageSize)
                .ToList();
        }

        pager.TotalItems = count;

        return (userMapsOrdered, pager);
    }

    private static MeiliSearchUserMap ConvertToUserMap(UserCacheItem user)
    {
        var result = new MeiliSearchUserMap
        {
            Id = user.Id,
            Name = user.Name,
            Rank = user.ReputationPos,
            WishCountQuestions = user.WishCountQuestions,
            ContentLanguages = user.ContentLanguages.Select(l => l.GetCode()).ToList()
        };
        return result;
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class MeiliSearchUsers : IRegisterAsInstancePerLifetime
    {
        private List<UserCacheItem> _users = new();
        private int _count = 20;
        private MeiliSearchUsersResult _result;

        public async Task<ISearchUsersResult> RunAsync(
            string searchTerm)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            var index = client.Index(MeiliSearchKonstanten.User);
            _result = new MeiliSearchUsersResult();

            _result.UserIds.AddRange(await LoadSearchResults(searchTerm, index));

            return _result;
        }

        private bool IsReloadRequired(int searchResultCount, int usersCount) //todo advanced class
        {
            if (searchResultCount == _count && usersCount < 5)
            {
                return true;
            }
            return false;
        }

        private async Task<List<int>> LoadSearchResults(string searchTerm, Index index)
        {
            var sq = new SearchQuery
            {
                Limit = _count
            };

            var userMaps =
                (await index.SearchAsync<MeiliSearchUserMap>(searchTerm, sq))
                .Hits;

            var userMapsSkip = userMaps
                .Skip(_count - 20)
                .ToList();

            FilterCacheItems(userMapsSkip);

            if (IsReloadRequired(userMaps.Count, _users.Count()))
            {
                _count += 20;
                await LoadSearchResults(searchTerm, index);
            };
            _result.Count = _users.Count;
            return _users
                .Select(c => c.Id)
                .Take(5)
                .ToList();
        }

        private void FilterCacheItems(List<MeiliSearchUserMap> userMaps)
        {
            var questionsTemp = EntityCache.GetUsersByIds(
                    userMaps.Select(c => c.Id))
                .ToList();
            _users.AddRange(questionsTemp);
            _users = _users
                .Distinct()
                .ToList();
        }
    }
}
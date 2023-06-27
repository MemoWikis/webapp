using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search
{
    public class MeiliSearchUsers : MeiliSearchHelper, IRegisterAsInstancePerLifetime
    {
        private List<UserCacheItem> _users = new();

        private MeiliSearchUsersResult _result;

        public async Task<ISearchUsersResult> RunAsync(
            string searchTerm)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            var index = client.Index(MeiliSearchKonstanten.Users);
            _result = new MeiliSearchUsersResult();

            _result.UserIds.AddRange(await LoadSearchResults(searchTerm, index));

            return _result;
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
                .Skip(_count - 20) //skip 0
                .ToList();

            AddUsers(userMapsSkip);

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

        public async Task<(List<MeiliSearchUserMap> searchResultUser, Pager pager)> GetUsersByPagerAsync(string searchTerm, Pager pager,SearchUsersOrderBy orderBy)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            var index = client.Index(MeiliSearchKonstanten.Users);

            var sq = new SearchQuery
            {
                Limit = 1000
            };

            var userMaps =
                    (await index.SearchAsync<MeiliSearchUserMap>(searchTerm, sq))
                    .Hits;

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
             
            var userMapsSkip = userMapsOrdered
                .Skip(pager.LowerBound - 1)
                .Take(pager.PageSize)
                .ToList();
            pager.TotalItems = userMaps.Count;

            return (userMapsSkip, pager);
        }
    }
}
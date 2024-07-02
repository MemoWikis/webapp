using Meilisearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search
{
    public class MeiliSearchUsers : MeiliSearchHelper, IRegisterAsInstancePerLifetime
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<UserCacheItem> _users = new();

        private MeiliSearchUsersResult _result;

        public MeiliSearchUsers(
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ISearchUsersResult> RunAsync(
            string searchTerm)
        {
            var client = new MeilisearchClient(MeiliSearchConstants.Url,
                MeiliSearchConstants.MasterKey);
            var index = client.Index(MeiliSearchConstants.Users);
            _result = new MeiliSearchUsersResult(_httpContextAccessor, _webHostEnvironment);

            _result.UserIds.AddRange(await LoadSearchResults(searchTerm, index));

            return _result;
        }

        private async Task<List<int>> LoadSearchResults(string searchTerm, Meilisearch.Index index)
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
            GetUsersByPagerAsync(string searchTerm, Pager pager, SearchUsersOrderBy orderBy)
        {
            List<MeiliSearchUserMap> userMaps = new List<MeiliSearchUserMap>(); 

            if (string.IsNullOrEmpty(searchTerm))
            {
                userMaps = EntityCache.GetAllUsers().Select(ConvertToUserMap).ToList(); 
            }
            else
            {
                var client = new MeilisearchClient(MeiliSearchConstants.Url,
                    MeiliSearchConstants.MasterKey);
                var index = client.Index(MeiliSearchConstants.Users);

                var sq = new SearchQuery
                {
                    Limit = 100
                };

                userMaps =
                    (await index.SearchAsync<MeiliSearchUserMap>(searchTerm, sq))
                    .Hits.ToList();
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

            var userMapsSkip = userMapsOrdered
                .Skip(pager.LowerBound - 1)
                .Take(pager.PageSize)
                .ToList();
            pager.TotalItems = userMaps.Count;

            return (userMapsSkip, pager);
        }

        private static MeiliSearchUserMap ConvertToUserMap(UserCacheItem user)
        {
            var result = new MeiliSearchUserMap
            {
                Id = user.Id,
                Name = user.Name,
                Rank = user.ReputationPos,
                WishCountQuestions = user.WishCountQuestions
            };
            return result;
        }
    }
}
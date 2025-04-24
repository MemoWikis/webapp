using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexUser : IRegisterAsInstancePerLifetime
    {
        private readonly UserReadingRepo _userReadingRepo;
        private readonly MeilisearchClient _client;

        public MeiliSearchReIndexUser(UserReadingRepo userReadingRepo)

        {
            _userReadingRepo = userReadingRepo;
            _client = new MeilisearchClient(MeiliSearchConstants.Url,
                MeiliSearchConstants.MasterKey);
        }

        public async Task RunAll()
        {
            await _client.DeleteIndexAsync(MeiliSearchConstants.Users);
            //await _client.DeleteIndexAsync("MeiliSearchPage");

            var allUser = _userReadingRepo.GetAll();
            var meiliSearchUserList = new List<MeiliSearchUserMap>();

            foreach (var user in allUser)
                meiliSearchUserList.Add(MeiliSearchToUserMap.Run(user));

            await _client.CreateIndexAsync(MeiliSearchConstants.Users);
            var index = _client.Index(MeiliSearchConstants.Users);
            await index.UpdateFilterableAttributesAsync(new[] { "ContentLanguages" });

            await index.AddDocumentsAsync(meiliSearchUserList);
        }

        public async Task RunAllCache()
        {
            await _client.DeleteIndexAsync(MeiliSearchConstants.Users);
            //await _client.DeleteIndexAsync("MeiliSearchPage");

            var allUser = EntityCache.GetAllUsers();
            var meiliSearchUserList = new List<MeiliSearchUserMap>();

            foreach (var user in allUser)
                meiliSearchUserList.Add(MeiliSearchToUserMap.Run(user));

            await _client.CreateIndexAsync(MeiliSearchConstants.Users);
            var index = _client.Index(MeiliSearchConstants.Users);
            await index.UpdateFilterableAttributesAsync(new[] { "ContentLanguages" });

            await index.AddDocumentsAsync(meiliSearchUserList);
        }

        public async Task Run(UserCacheItem user)
        {
            var meiliSearchUser = MeiliSearchToUserMap.Run(user);
            var index = _client.Index(MeiliSearchConstants.Users);
            await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { meiliSearchUser });
        }

        public async Task Run(User user)
        {
            var meiliSearchUser = MeiliSearchToUserMap.Run(user);
            var index = _client.Index(MeiliSearchConstants.Users);
            await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { meiliSearchUser });
        }
    }
}
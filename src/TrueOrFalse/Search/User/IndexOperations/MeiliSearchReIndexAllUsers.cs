using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexAllUsers : IRegisterAsInstancePerLifetime
    {
        private readonly UserReadingRepo _userReadingRepo;
        private readonly MeilisearchClient _client;

        public MeiliSearchReIndexAllUsers(UserReadingRepo userReadingRepo)

        {
            _userReadingRepo = userReadingRepo;
            _client = new MeilisearchClient(MeiliSearchConstants.Url,
                MeiliSearchConstants.MasterKey);
        }

        public async Task Run()
        {
            await _client.DeleteIndexAsync(MeiliSearchConstants.Users);
            await _client.DeleteIndexAsync("MeiliSearchCategory");

            var allUser = _userReadingRepo.GetAll();
            var listMeilieSearchUser = new List<MeiliSearchUserMap>();

            foreach (var user in allUser)
                listMeilieSearchUser.Add(MeiliSearchToUserMap.Run(user));

            await _client.CreateIndexAsync(MeiliSearchConstants.Users);
            var index = _client.Index(MeiliSearchConstants.Users);
            await index.AddDocumentsAsync(listMeilieSearchUser);
        }
    }
}
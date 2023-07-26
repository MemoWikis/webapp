using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey); 
        }

        public async Task Run()
        {
            await _client.DeleteIndexAsync(MeiliSearchKonstanten.Users);
            await _client.DeleteIndexAsync("MeiliSearchCategory"); 


            var allUser = _userReadingRepo.GetAll();
            var listMeilieSearchUser = new List<MeiliSearchUserMap>(); 

            foreach (var user in allUser)
                listMeilieSearchUser.Add(MeiliSearchToUserMap.Run(user));

            await _client.CreateIndexAsync(MeiliSearchKonstanten.Users);
            var index = _client.Index(MeiliSearchKonstanten.Users); 
            await index.AddDocumentsAsync(listMeilieSearchUser);
        }
    }
}
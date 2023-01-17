using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexAllUsers : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepo _userRepo;
        private readonly MeilisearchClient _client; 

        public MeiliSearchReIndexAllUsers(UserRepo userRepo) 
           
        {
            _userRepo = userRepo;
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey); 
        }

        public async Task Run()
        {
            await _client.DeleteIndexAsync(MeiliSearchKonstanten.User);
            await _client.DeleteIndexAsync("MeiliSearchCategory"); 


            var allUser = _userRepo.GetAll();
            var listMeilieSearchUser = new List<MeiliSearchUserMap>(); 

            foreach (var user in allUser)
                listMeilieSearchUser.Add(MeiliSearchToUserMap.Run(user));

            await _client.CreateIndexAsync(MeiliSearchKonstanten.User);
            var index = _client.Index(MeiliSearchKonstanten.User); 
            await index.AddDocumentsAsync(listMeilieSearchUser);
        }
    }
}
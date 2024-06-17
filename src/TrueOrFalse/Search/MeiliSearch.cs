using Meilisearch;

namespace TrueOrFalse.Search
{
    public  class MeiliSearch : IRegisterAsInstancePerLifetime
    {
        public MeilisearchClient Client;


        public MeiliSearch()
        {
            Client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
        }
           
    }
}

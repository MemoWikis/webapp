using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

internal class MeiliSearchBase : MeiliSearchTestConstants
{
    protected MeilisearchClient client;

    public MeiliSearchBase()
    {
        client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
    }

    protected async Task DeleteTestUser()
    {
        var deleteIndexTaskId = (await client.DeleteIndexAsync(UsersTest)).TaskUid; 
        await client.WaitForTaskAsync(deleteIndexTaskId);

        var createIndexId = (await client.CreateIndexAsync(UsersTest)).TaskUid;
        await client.WaitForTaskAsync(createIndexId);
    }
}
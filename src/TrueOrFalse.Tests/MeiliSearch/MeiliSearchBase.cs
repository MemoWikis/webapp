using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

internal class MeiliSearchBase
{
    protected MeilisearchClient client;

    public MeiliSearchBase()
    {
        client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
    }

    protected async Task DeleteUserTest()
    {
        var deleteIndexTaskId = (await client.DeleteIndexAsync(MeiliSearchTestConstants.UsersTest)).TaskUid; 
        await client.WaitForTaskAsync(deleteIndexTaskId);

        var createIndexId = (await client.CreateIndexAsync(MeiliSearchTestConstants.UsersTest)).TaskUid;
        await client.WaitForTaskAsync(createIndexId);
    }

    private bool IsIndexAvailable(string index, IEnumerable<string> indexNames)
    {
        return indexNames.Contains(index);
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

internal class MeiliSearchBaseTests : MeiliSearchTestConstants
{
    protected MeilisearchClient client;

    internal MeiliSearchBaseTests()
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

    protected async Task DeleteCategories()
    {
        var deleteIndexTaskId = (await client.DeleteIndexAsync(CategoriesTest)).TaskUid;
        await client.WaitForTaskAsync(deleteIndexTaskId);

        var createIndexId = (await client.CreateIndexAsync(CategoriesTest)).TaskUid;
        await client.WaitForTaskAsync(createIndexId);
    }

    protected async Task DeleteQuestions()
    {
        var deleteIndexTaskId = (await client.DeleteIndexAsync(QuestionsTest)).TaskUid;
        await client.WaitForTaskAsync(deleteIndexTaskId);

        var createIndexId = (await client.CreateIndexAsync(QuestionsTest)).TaskUid;
        await client.WaitForTaskAsync(createIndexId);
    }
}
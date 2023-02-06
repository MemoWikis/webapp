using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Mapping;
using NUnit.Framework;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

internal class MeiliSearchUserTests : MeiliSearchBase
{
    [Test(Description = "Set TestUser in MeiliSearch")]
    public async Task CreateUserTest()
    {
        //construction
        await DeleteUserTest();
        var user = new User
        {
            Id = 12,
            Name = "Daniel",
            ActivityLevel = 5,
            WishCountQuestions = 2000
        };

        //Execution
        var createDocument = (await MeiliSearchUsersDatabaseOperations.CreateAsync(user, MeiliSearchTestConstants.UsersTest).ConfigureAwait(false)).TaskUid;
        await client.WaitForTaskAsync(createDocument);
        var searchQuery = new SearchQuery
        {
            Limit = 100
        };
        var index = client.Index(MeiliSearchTestConstants.UsersTest);
        var result = (await index.SearchAsync<MeiliSearchUserMap>(user.Name, searchQuery).ConfigureAwait(false)).Hits.ToList();
        var userMap = result.First();

        //Tests 
        Assert.AreEqual(result.GetType(), typeof(List<MeiliSearchUserMap>));
        Assert.True(result.Count == 1);

        Assert.AreEqual(userMap.Name, user.Name);
        Assert.True((DateTime.Now - userMap.DateCreated).Ticks > 0);
        Assert.AreEqual(userMap.Id, user.Id);
        Assert.AreEqual(userMap.Rank, user.ActivityLevel);
        Assert.AreEqual(userMap.WishCountQuestions, user.WishCountQuestions);
    }
}
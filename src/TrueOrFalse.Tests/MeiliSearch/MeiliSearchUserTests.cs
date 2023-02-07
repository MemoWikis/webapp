using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

internal class MeiliSearchUserTests : MeiliSearchBase
{
    [Test(Description = "Set TestUser in MeiliSearch")]
    public async Task CreateUserTest()
    {
        //construction
        await DeleteTestUser();
        var user = new User
        {
            Id = 12,
            Name = "Daniel",
            ActivityLevel = 5,
            WishCountQuestions = 2000
        };

        //Execution
        var taskId = (await MeiliSearchUsersDatabaseOperations.CreateAsync(user, MeiliSearchTestConstants.UsersTest).ConfigureAwait(false)).TaskUid;
        await client.WaitForTaskAsync(taskId);
     
        var index = client.Index(MeiliSearchTestConstants.UsersTest);
        var result = (await index.SearchAsync<MeiliSearchUserMap>(user.Name).ConfigureAwait(false)).Hits.ToList();
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

    [Test(Description = "Update TestUser in MeiliSearch")]
    public async Task UpdateUserTest()
    {
        //construction
        await DeleteTestUser();
        var user = new User
        {
            Id = 12,
            Name = "Daniel",
            ActivityLevel = 5,
            WishCountQuestions = 2000
        };

        //Execution
        var taskId = (await MeiliSearchUsersDatabaseOperations.CreateAsync(user, MeiliSearchTestConstants.UsersTest).ConfigureAwait(false)).TaskUid;
        await client.WaitForTaskAsync(taskId);

        user.Name = "Daniela";
        taskId = (await MeiliSearchUsersDatabaseOperations.UpdateAsync(user, MeiliSearchTestConstants.UsersTest).ConfigureAwait(false)).TaskUid;
        await client.WaitForTaskAsync(taskId);

        var index = client.Index(MeiliSearchTestConstants.UsersTest);
        var result = (await index.SearchAsync<MeiliSearchUserMap>(user.Name).ConfigureAwait(false)).Hits.ToList();
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


    [Test(Description = "Delete TestUser in MeiliSearch")]
    public async Task DeleteUserTest()
    {
        //construction
        await DeleteTestUser();
        var user = new User
        {
            Id = 12,
            Name = "Daniel",
            ActivityLevel = 5,
            WishCountQuestions = 2000
        };

        //Execution
        var taskId = (await MeiliSearchUsersDatabaseOperations.CreateAsync(user, MeiliSearchTestConstants.UsersTest).ConfigureAwait(false)).TaskUid;
        await client.WaitForTaskAsync(taskId);

      
        taskId = (await MeiliSearchUsersDatabaseOperations.DeleteAsync(user, MeiliSearchTestConstants.UsersTest).ConfigureAwait(false)).TaskUid;
        await client.WaitForTaskAsync(taskId);

        var index = client.Index(MeiliSearchTestConstants.UsersTest);
        var result = (await index.SearchAsync<MeiliSearchUserMap>(user.Name).ConfigureAwait(false)).Hits.ToList();
        var userMap = result.FirstOrDefault();
        //Tests 
        Assert.True(result.IsNullOrEmpty());
        Assert.True(userMap == null);
    }
}
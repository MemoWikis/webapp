[TestFixture]
internal class TestHarnessTests : BaseTestHarness
{
    [Test]
    public async Task Should_create_testHarness_with_host_and_db_access()
    {
        await ReloadCaches();
        
        // arrange 
        var userWritRepo = _testHarness.R<UserWritingRepo>();
        var user = new User
        {
            Name = "TestUser",
            EmailAddress = "test@test.de"
        };

        // act
        userWritRepo.Create(user);

        await Verify(new
        {
            allDbUsers = await _testHarness.DbData.AllUsersAsync(),
            allSearchIndexUsers = await _testHarness.SearchData.GetAllUsers()
        });
    }

    [Test]
    public async Task Should_create_testHarness_and_make_api_call()
    {
        await ReloadCaches();

        string result = await _testHarness.ApiCall("apiVue/App/GetCurrentUser");

        await Verify(new { formattedJson = result });
    }

    //[Test]
    //public async Task TestHarness_CanUseSpecificScenarioImage()
    //{
    //    await using var harness = await TestHarness.CreateWithScenarioImageAsync(ScenarioImageConstants.BaseName);

    //    // Assert
    //    Assert.That(harness.Client, Is.Not.Null);
    //    Assert.That(harness.ConnectionString, Is.Not.Empty);
    //}
}


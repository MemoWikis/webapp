[TestFixture]
internal class TestHarnessTests : BaseTestHarness
{
    [Test]
    public async Task Should_create_testHarness_with_host_and_db_access()
    {
        await ClearData();
        
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
        await ClearData();

        string result = await _testHarness.ApiGet("apiVue/App/GetCurrentUser");

        await Verify(new { formattedJson = result });
    }
}
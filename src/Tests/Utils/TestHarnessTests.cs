using System.Diagnostics.CodeAnalysis;

[TestFixture]
internal class TestHarnessTests
{
    private readonly TestHarness _testHarness = new(enablePerfLogging: true);

    [OneTimeSetUp]
    public async Task OneTimeSetUp() => await _testHarness.InitAsync(keepData: false);

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _testHarness.DisposeAsync();

    [Test]
    public async Task Should_create_testHarness_and_with_host_and_db_access()
    {
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
        string result = await _testHarness.ApiCall("apiVue/App/GetCurrentUser");

        await Verify(new
        {
            formattedJson = result
        });
    }
}
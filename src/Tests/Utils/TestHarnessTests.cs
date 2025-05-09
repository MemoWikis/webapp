[TestFixture]
internal class TestHarnessTests
{
    [Test]
    public async Task Should_create_testHarness_and_with_host_and_db_access() 
    {
        var testHarness = new TestHarness(enablePerfLogging: true);
        await testHarness.InitAsync();

        var userWritRepo = testHarness.R<UserWritingRepo>();

        var user = new User
        {
            Name = "TestUser",
            EmailAddress = "test@test.de"
        };
        
        userWritRepo.Create(user);
    }
}
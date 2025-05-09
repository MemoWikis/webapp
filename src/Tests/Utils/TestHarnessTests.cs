[TestFixture]
internal class TestHarnessTests
{
    [Test]
    public async Task Should_create_testHarness_and_with_host_and_db_access() 
    {
        var testHarness = new TestHarness();
        await testHarness.InitAsync();
    }
}
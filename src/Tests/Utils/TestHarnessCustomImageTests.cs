internal class TestHarnessCustomImageTests : BaseTestHarness
{
    public TestHarnessCustomImageTests() => _useTinyScenario = true;

    [Test]
    public async Task TestHarness_CanUseSpecificScenarioImage()
    {
        await Verify(new
        {
            users = await _testHarness.DbData.AllUsersAsync(),
            pages = await _testHarness.DbData.AllPagesAsync()
        });
    }
}
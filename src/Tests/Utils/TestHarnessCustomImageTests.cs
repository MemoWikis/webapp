internal class TestHarnessCustomImageTests : BaseTestHarness
{
    public TestHarnessCustomImageTests() => _useTinyScenario = true;

    [Test]
    public async Task TestHarness_CanUseSpecificScenarioImage()
    {
        await Verify(new
        {
            usersSummary = await _testHarness.DbData.AllUsersAsync()
        });
    }
}
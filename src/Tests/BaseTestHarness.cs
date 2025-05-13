[TestFixture]
internal class BaseTestHarness
{
#pragma warning disable NUnit1032
    private TestHarness _testHarness = new();
#pragma warning restore NUnit1032

    protected T R<T>() where T : notnull => _testHarness.Resolve<T>();

    [OneTimeSetUp]
    public async Task OneTimeSetUp() => await _testHarness.InitAsync();

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _testHarness.DisposeAsync();

    public async Task ReloadCaches() => 
        await _testHarness.InitAsync(keepData: true);

    public async Task ClearData()
    {
        await _testHarness.DisposeAsync();
        _testHarness = new TestHarness();
        await _testHarness.InitAsync(keepData: false);
    }

    
    protected ContextPage NewPageContext(bool addContextUser = true)
        => new(_testHarness, addContextUser);

    protected ContextQuestion NewQuestionContext(bool persistImmediately = false)
        => new(_testHarness, persistImmediately);
}
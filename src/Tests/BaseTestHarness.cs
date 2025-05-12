using NHibernate;

[TestFixture]
internal class BaseTestHarness
{
    private TestHarness _testHarness = new();

    protected T R<T>() where T : notnull => _testHarness.Resolve<T>();

    protected ContextPage NewPageContext(bool addContextUser = true) 
        => new(_testHarness, addContextUser);

    protected ContextQuestion NewQuestionContext(bool persistImmediately = false) 
        => new(_testHarness, persistImmediately);

    [OneTimeSetUp]
    public async Task OneTimeSetUp() => await _testHarness.InitAsync();

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _testHarness.DisposeAsync();

    public async Task ReloadCaches()
    {
        _testHarness = new();
        await _testHarness.InitAsync(keepData: true);
    }
}
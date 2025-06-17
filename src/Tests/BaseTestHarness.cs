[TestFixture]
internal class BaseTestHarness
{
#pragma warning disable NUnit1032
    private Lazy<Task<TestHarness>> _lazyTestHarness = new(() => TestHarness.CreateAsync());
    protected TestHarness _testHarness = null!; // Will be set in OneTimeSetUp
#pragma warning restore NUnit1032

    protected T R<T>() where T : notnull => _testHarness.Resolve<T>();

    [OneTimeSetUp]
    public async Task OneTimeSetUp() => _testHarness = await _lazyTestHarness.Value;

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_lazyTestHarness.IsValueCreated)
        {
            await (await _lazyTestHarness.Value).DisposeAsync();
        }
    }

    public async Task ReloadCaches()
        => await _testHarness.InitAsync(keepData: true);

    public async Task ClearData()
    {
        if (_lazyTestHarness.IsValueCreated)
            await (await _lazyTestHarness.Value).DisposeAsync();

        // Reset the lazy initializer to allow creating a new instance
        _lazyTestHarness = new Lazy<Task<TestHarness>>(() => TestHarness.CreateAsync());
        _testHarness = await _lazyTestHarness.Value;
    }

    protected ContextPage NewPageContext(bool addContextUser = true, bool createFeaturedRootPage = false)
        => new(_testHarness, addContextUser, createFeaturedRootPage);

    protected ContextQuestion NewQuestionContext(bool persistImmediately = false)
        => new(_testHarness, persistImmediately);
}
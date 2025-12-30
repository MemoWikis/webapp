using Serilog;

[TestFixture]
internal class BaseTestHarness : IDisposable, IAsyncDisposable
{
    protected bool _useTinyScenario = false;
    protected bool _skipDefaultUsers = false;

    protected TestHarness _testHarness = null!; // Will be set in OneTimeSetUp

    protected T R<T>() where T : notnull => _testHarness.Resolve<T>();

    [OneTimeSetUp]
    public async Task OneTimeSetUp() => await CreateTestHarness();

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _testHarness.DisposeAsync();

    public async Task ReloadCaches()
        => await _testHarness.InitAsync(keepData: true);

    public async Task ClearData()
    {
        await _testHarness.DisposeAsync();
        await CreateTestHarness();
    }

    private async Task CreateTestHarness()
    {
        Settings.IsRunningTests = true;

        if (_useTinyScenario)
            _testHarness = await TestHarness.CreateWithTinyScenario();
        else
            _testHarness = await TestHarness.CreateAsync(skipDefaultUsers: _skipDefaultUsers);
    }

    protected ContextPage NewPageContext(bool addContextUser = true)
        => new(_testHarness, addContextUser);

    protected ContextQuestion NewQuestionContext(bool persistImmediately = false)
        => new(_testHarness, persistImmediately);

    public void Dispose()
    {
        _testHarness.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _testHarness.DisposeAsync();
    }
}
[TestFixture]
internal class TestHarnessTests
{
    [Test]
    public async Task Foo() 
    {
        var testHarness = new TestHarness();
        await testHarness.InitAsync();
    }
}

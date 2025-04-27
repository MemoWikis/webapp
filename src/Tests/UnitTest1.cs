public class Tests : BaseTest
{
    public Tests()
    {
    }

    [Test]
    public void Test1()
    {
        ContextPage.New().Add("A").Persist();
    }
}
using NUnit.Framework;
using TrueOrFalse;

[SetUpFixture]
public class GlobalTest
{
    [OneTimeSetUp]
    public void InitializeDb()
    {
        //BaseTest.InitializeContainer();
        //SessionFactory.BuildSchema();
    }
}
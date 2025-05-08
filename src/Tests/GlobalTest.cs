using Microsoft.Extensions.Configuration;

[SetUpFixture]
public class GlobalTest
{
    [OneTimeSetUp]
    public void InitializeDb()
    {
        Settings.Initialize(GetConfiguration());
        SessionFactory.BuildTestConfiguration();
        //BaseTest.InitializeContainer();
        SessionFactory.BuildSchema();
    }

    private IConfiguration GetConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.Test.json");
        return configurationBuilder.Build();
    }
}
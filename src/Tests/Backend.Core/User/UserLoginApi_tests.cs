internal class UserLoginApi_tests : BaseTestHarness
{
    private UserLoginApiWrapper _userLoginApi => _testHarness.ApiUserLogin;

    [Test]
    public async Task Login_WithValidCredentials_ReturnsSuccess()
    {
        // Act
        var result = await _userLoginApi.Login("sessionUser@dev.test", "test123");

        // Assert
        await Verify(result);
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ReturnsFailure()
    {
        // Act
        var result = await _userLoginApi.Login("invalidUser@dev.test", "wrongPassword");

        // Assert
        await Verify(result);
    }

    [Test]
    public async Task LoginAsSessionUser_WithCreateWiki_CreatesWikiAutomatically()
    {
        // Arrange - Clear any existing wikis
        await ClearData();

        // Act
        var result = await _userLoginApi.LoginAsSessionUser(createWiki: true);

        // Assert
        var wikis = EntityCache.GetWikisByUserId(_testHarness.DefaultSessionUserId);
        await Verify(new
        {
            loginResult = result,
            wikiCount = wikis.Count,
            firstWikiName = wikis.FirstOrDefault()?.Name
        });
    }
}
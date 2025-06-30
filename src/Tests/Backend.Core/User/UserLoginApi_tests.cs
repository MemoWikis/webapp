internal class UserLoginApi_tests : BaseTestHarness
{
    private UserLoginApiWrapper _userLoginApi => _testHarness.ApiUserLogin;

    private void SetupSessionUserWiki()
    {
        // Arrange
        var userRepo = R<UserReadingRepo>();
        var sessionUserDbUser = userRepo.GetById(_testHarness.DefaultSessionUserId)!;

        // Create personal wiki for session user
        var pageContext = NewPageContext(addContextUser: false);
        pageContext
            .Add("SessionUser Personal Wiki", creator: sessionUserDbUser, isWiki: true)
            .Persist();
    }

    [Test]
    public async Task Login_WithValidCredentials_ReturnsSuccess()
    {
        SetupSessionUserWiki();

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
}
using System.Text;
using System.Text.Json;

public class UserLoginApiWrapper
{
    private readonly TestHarness _testHarness;

    public UserLoginApiWrapper(TestHarness testHarness)
    {
        _testHarness = testHarness;
    }

    /// <summary>
    /// Login as the session user for authenticated API testing
    /// </summary>
    public async Task LoginAsSessionUser()
    {
        var loginRequest = new LoginRequest("sessionUser@dev.test", "test123", false);
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        var httpResponse = await _testHarness.Client.PostAsync("apiVue/UserStore/Login", jsonContent);
        httpResponse.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Logout the session user
    /// </summary>
    public async Task LogoutSessionUser()
    {
        var httpResponse = await _testHarness.Client.PostAsync("apiVue/UserStore/LogOut", null);
        httpResponse.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Login with custom credentials
    /// </summary>
    public async Task<UserStoreController.LoginResponse> Login(string email, string password, bool persistentLogin = false)
    {
        var loginRequest = new LoginRequest(email, password, persistentLogin);
        return await _testHarness.ApiPost<UserStoreController.LoginResponse>("apiVue/UserStore/Login", loginRequest);
    }

    /// <summary>
    /// Create a personal wiki page for a user. This is needed because login functionality requires users to have a firstWikiId.
    /// </summary>
    public void CreatePersonalWikiForUser(int userId, string? wikiName = null)
    {
        var userRepo = _testHarness.Resolve<UserReadingRepo>();
        var user = userRepo.GetById(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        var pageContext = new ContextPage(_testHarness, addContextUser: false);
        pageContext
            .Add(wikiName ?? $"{user.Name} Personal Wiki", creator: user, isWiki: true)
            .Persist();
    }

    /// <summary>
    /// Create a personal wiki for the session user (ID 1)
    /// </summary>
    public void CreatePersonalWikiForSessionUser()
    {
        CreatePersonalWikiForUser(1, "SessionUser Personal Wiki");
    }
}
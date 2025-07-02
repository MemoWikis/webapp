using System.Text.Json;

public class UserLoginApiWrapper
{
    private readonly TestHarness _testHarness;
    private string? _persistentCookie;

    public UserLoginApiWrapper(TestHarness testHarness)
    {
        _testHarness = testHarness;
    }

    /// <summary>
    /// Login as the session user for authenticated API testing
    /// Uses the Login method and sets persistent cookie for subsequent requests
    /// </summary>
    /// <param name="createWiki">If true, creates a personal wiki for the session user if it doesn't exist</param>
    public async Task<UserStoreController.LoginResponse> LoginAsSessionUser(bool createWiki = false)
    {
        if (createWiki)
        {
            SetupSessionUserWiki();
        }

        return await Login(_testHarness.DefaultSessionUser.EmailAddress, _testHarness.DefaultSessionUserPassword, true);
    }

    /// <summary>
    /// Logout the session user
    /// </summary>
    public async Task LogoutSessionUser()
    {
        var httpResponse = await _testHarness.Client.PostAsync("apiVue/UserStore/LogOut", null);
        httpResponse.EnsureSuccessStatusCode();

        // Clear the persistent cookie after logout
        _persistentCookie = null;
        _testHarness.Cookies.Clear();
    }

    /// <summary>
    /// Login with custom credentials
    /// </summary>
    public async Task<UserStoreController.LoginResponse> Login(string email, string password, bool persistentLogin = true)
    {
        var loginRequest = new LoginRequest(email, password, persistentLogin);

        // Use the TestHarness ApiCall to get the raw response so we can access headers
        var httpResponse = await _testHarness.ApiCall("apiVue/UserStore/Login", loginRequest);
        httpResponse.EnsureSuccessStatusCode();

        // Deserialize the response using the same logic as ApiPost
        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<UserStoreController.LoginResponse>(responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        // If login was successful and persistent login was requested, extract the persistent cookie
        if (loginResponse.Success && persistentLogin)
        {
            // Extract the persistent cookie from the Set-Cookie headers
            if (httpResponse.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
            {
                foreach (var cookieHeader in setCookieHeaders)
                {
                    if (cookieHeader.Contains("persistentLogin="))
                    {
                        // Extract the cookie value: "persistentLogin=value; Path=/; ..."
                        var cookieParts = cookieHeader.Split(';')[0].Split('=');
                        if (cookieParts.Length == 2 && cookieParts[0].Trim() == "persistentLogin")
                        {
                            _persistentCookie = cookieParts[1].Trim();
                            // Store the cookie in the TestHarness cookies collection
                            _testHarness.AddOrUpdateCookie("persistentLogin", _persistentCookie);
                            break;
                        }
                    }
                }
            }
        }

        return loginResponse;
    }

    /// <summary>
    /// Sets up a personal wiki for the session user if it doesn't already exist
    /// </summary>
    public void SetupSessionUserWiki(ContextPage? pageContext = null)
    {
        var userRepo = _testHarness.R<UserReadingRepo>();
        var sessionUserDbUser = userRepo.GetById(_testHarness.DefaultSessionUserId)!;

        // Check if session user already has a wiki
        var existingWikis = EntityCache.GetWikisByUserId(_testHarness.DefaultSessionUserId);
        if (existingWikis.Any())
        {
            return; // Wiki already exists, no need to create
        }

        // Create personal wiki for session user
        pageContext ??= _testHarness.NewPageContext(addContextUser: false);
        pageContext
            .Add("SessionUser Personal Wiki", creator: sessionUserDbUser, isWiki: true)
            .Persist();
    }
}
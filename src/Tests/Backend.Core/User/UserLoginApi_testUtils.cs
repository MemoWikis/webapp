public class UserLoginApiWrapper
{
    private readonly TestHarness testHarness;

    public UserLoginApiWrapper(TestHarness testHarness)
    {
        this.testHarness = testHarness;
    }

    public async Task<LoginResult> Login(LoginRequest request)
    {
        // Adjust the endpoint and DTOs as needed
        return await testHarness.ApiPostJson<LoginRequest, LoginResult>(
            "/apiVue/Auth/Login",
            request);
    }
}

// Example DTOs, adjust as needed
public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
    public string ErrorMessage { get; set; }
}
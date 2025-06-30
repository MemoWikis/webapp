using System.Threading.Tasks;


public class UserLoginApi_tests : TestHarness
{
    private readonly UserLoginApiWrapper userLoginApiWrapper;

    public UserLoginApi_tests()
    {
        var testHarness = new TestHarness();
        userLoginApiWrapper = new UserLoginApiWrapper(testHarness);
    }

    [Test]
    public async Task Login_WithValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var request = new LoginRequest
        {
            UserName = "validUser",
            Password = "validPassword"
        };

        // Act
        var result = await userLoginApiWrapper.Login(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        await Verify(result);
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ReturnsFailure()
    {
        // Arrange
        var request = new LoginRequest
        {
            UserName = "invalidUser",
            Password = "wrongPassword"
        };

        // Act
        var result = await userLoginApiWrapper.Login(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        await Verify(result);
    }
}
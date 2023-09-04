using Xunit;
using System;

public class EmailConfirmationServiceTests
{
    private readonly EmailConfirmationService _emailConfirmationService;

    public EmailConfirmationServiceTests()
    {
        _emailConfirmationService = new EmailConfirmationService(null);
    }

    [Fact]
    public void CreateEmailConfirmationToken_ReturnsExpectedFormat()
    {
        var user = new User { Id = 1, DateCreated = DateTime.UtcNow, PasswordHashedAndSalted = "1231adhb24" };
        var token = EmailConfirmationService.CreateEmailConfirmationToken(user);

        // Split the token and make sure there are two parts
        var parts = token.Split('-');
        Assert.Equal(2, parts.Length);

        // Make sure the second part of the token matches the user's ID
        Assert.Equal(user.Id.ToString(), parts[1]);
    }

    [Fact]
    public void TryConfirmEmailTest_ReturnsTrue_WhenTokenIsValid()
    {
        var user = new User { Id = 1, DateCreated = DateTime.UtcNow, PasswordHashedAndSalted = "1231adhb24" };
        var token = EmailConfirmationService.CreateEmailConfirmationToken(user);

        var result = _emailConfirmationService.TryConfirmEmailTest(token, user);

        Assert.True(result);
    }

    [Fact]
    public void TryConfirmEmailTest_ReturnsFalse_WhenTokenIsInvalid()
    {
        var user = new User { Id = 1, DateCreated = DateTime.UtcNow, PasswordHashedAndSalted = "1231adhb24" };
        var token = "invalid-token";

        var result = _emailConfirmationService.TryConfirmEmailTest(token, user);

        Assert.False(result);
    }

    [Fact]
    public void Run_ReturnsExpectedUrl()
    {
        var user = new User { Id = 1, DateCreated = DateTime.UtcNow, PasswordHashedAndSalted = "1231adhb24" };
        var expectedUrl = "https://memucho.de/EmailBestaetigen/" + EmailConfirmationService.CreateEmailConfirmationToken(user);

        var result = CreateEmailConfirmationLink.Run(user);

        Assert.Equal(expectedUrl, result);
    }
}
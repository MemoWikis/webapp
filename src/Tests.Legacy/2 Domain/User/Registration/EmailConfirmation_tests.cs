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


    [Fact]
    public void CreateEmailConfirmationToken_ShouldReturnValidToken_WhenDateCreatedIsISOFormat()
    {
        var user = new User
        {
            DateCreated = DateTime.Parse("2023-09-23T00:36:27.3876261+02:00"),
            PasswordHashedAndSalted = "abcdef",
            Id = 1
        };

        var token = EmailConfirmationService.CreateEmailConfirmationToken(user);

        Assert.NotNull(token);
        // You might want more specific assertions here, e.g. length checks or format checks.
    }

    [Fact]
    public void CreateEmailConfirmationToken_ShouldReturnValidToken_WhenDateCreatedIsSimpleFormat()
    {
        var user = new User
        {
            DateCreated = DateTime.Parse("2023-09-23 00:36:27"),
            PasswordHashedAndSalted = "abcdef",
            Id = 1
        };

        var token = EmailConfirmationService.CreateEmailConfirmationToken(user);

        Assert.NotNull(token);
        // Again, you might want more specific assertions here.
    }

    [Fact]
    public void CreateEmailConfirmationToken_ShouldThrow_WhenDateCreatedIsInvalidFormat()
    {
        // This will throw a FormatException because "InvalidDate" cannot be parsed to a DateTime.
        Assert.Throws<FormatException>(() =>
        {
            var user = new User
            {
                DateCreated = DateTime.Parse("InvalidDate"),
                PasswordHashedAndSalted = "abcdef",
                Id = 1
            };

            EmailConfirmationService.CreateEmailConfirmationToken(user);
        });
    }

    [Fact]
    public void CreateEmailConfirmationToken_ShouldReturnSameToken_ForAllDateFormats()
    {
        var userIsoFormat = new User
        {
            DateCreated = DateTime.Parse("2023-09-23T00:36:27.3876261+02:00"),
            PasswordHashedAndSalted = "abcdef",
            Id = 1
        };

        var userSimpleFormat = new User
        {
            DateCreated = DateTime.Parse("2023-09-23 00:36:27"),
            PasswordHashedAndSalted = "abcdef",
            Id = 1
        };

        var userDayFirstFormat = new User
        {
            DateCreated = DateTime.Parse("23/09/2023 00:36:27"),
            PasswordHashedAndSalted = "abcdef",
            Id = 1
        };

        var tokenIso = EmailConfirmationService.CreateEmailConfirmationToken(userIsoFormat);
        var tokenSimple = EmailConfirmationService.CreateEmailConfirmationToken(userSimpleFormat);
        var tokenDayFirst = EmailConfirmationService.CreateEmailConfirmationToken(userDayFirstFormat);

        Assert.Equal(tokenIso, tokenSimple);
        Assert.Equal(tokenSimple, tokenDayFirst);
    }
}
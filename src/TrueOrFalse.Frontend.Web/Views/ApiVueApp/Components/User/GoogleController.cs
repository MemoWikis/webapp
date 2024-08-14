using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueOrFalse.Domain.User;

namespace VueApp;

public class GoogleController(
    UserReadingRepo _userReadingRepo,
    FrontEndUserData _frontEndUserData,
    GoogleLogin _googleLogin) : Controller
{
    public readonly record struct LoginRequest(string? credential, string? accessToken);

    public readonly record struct LoginResponse(
        bool Success,
        string MessageKey,
        FrontEndUserData.CurrentUserData Data);

    [HttpPost]
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {

        var loginResult = await _googleLogin.Login(request.credential, request.accessToken);

        if (loginResult.Success)
            return new LoginResponse
            {
                Success = true,
                MessageKey = loginResult.MessageKey,
                Data = _frontEndUserData.Get()
            };

        return new LoginResponse
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.Default
        };
    }

    [HttpPost]
    public bool UserExists(string googleId) => _userReadingRepo.GoogleUserExists(googleId);

}
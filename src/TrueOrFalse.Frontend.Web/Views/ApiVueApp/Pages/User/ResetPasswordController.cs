using System;
using Antlr.Runtime;
using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace VueApp;

public class ResetPasswordController(
    PasswordRecoveryTokenValidator _passwordRecoveryTokenValidator,
    SessionUser _sessionUser,
    VueSessionUser _vueSessionUser,
    UserReadingRepo _userReadingRepo,
    UserWritingRepo _userWritingRepo,
    ISession _session) : Controller
{
    public record struct ResetPasswordResult(bool Success, string MessageKey);

    private ResetPasswordResult ValidateToken(string token)
    {
        var passwordToken = _passwordRecoveryTokenValidator.Run(token);
        var result = new ResetPasswordResult
        {
            Success = true
        };

        if (passwordToken == null)
        {
            result.Success = false;
            result.MessageKey = FrontendMessageKeys.Error.User.PasswordResetTokenIsInvalid;
        }
        else if ((DateTime.Now - passwordToken.DateCreated).TotalDays > 3)
        {
            result.Success = false;
            result.MessageKey = FrontendMessageKeys.Error.User.PasswordResetTokenIsExpired;
        }

        return result;
    }

    public readonly record struct ValidateResult(bool Success, string MessageKey);

    [HttpGet]
    public ValidateResult Validate([FromRoute] string token)
    {
        var validateToken = ValidateToken(token);
        return new ValidateResult
            { Success = validateToken.Success, MessageKey = validateToken.MessageKey };
    }

    public readonly record struct SetNewPasswordJson(string token, string password);

    public readonly record struct SetNewPasswordResult(
        bool Success,
        VueSessionUser.CurrentUserData Data,
        string MessageKey);

    [HttpPost]
    public SetNewPasswordResult SetNewPassword([FromBody] SetNewPasswordJson json)
    {
        var validationResult = ValidateToken(json.token);
        if (validationResult.Success == false)
        {
            return new SetNewPasswordResult
                { Success = validationResult.Success, MessageKey = validationResult.MessageKey };
        }

        var result = PasswordResetPrepare.Run(json.token, _session);

        if (json.password.Trim().Length < 5)
        {
            return new SetNewPasswordResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.PasswordTooShort
            };
        }

        var user = _userReadingRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(json.password.Trim(), user);
        _userWritingRepo.Update(user);

        _sessionUser.Login(user);
        return new SetNewPasswordResult
        {
            Success = true,
            Data = _vueSessionUser.GetCurrentUserData()
        };
    }
}
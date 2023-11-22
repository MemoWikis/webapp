using System;
using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace VueApp;

public class ResetPasswordController : BaseController
{
    private readonly PasswordRecoveryTokenValidator _passwordRecoveryTokenValidator;
    private readonly VueSessionUser _vueSessionUser;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly ISession _session;

    public ResetPasswordController(PasswordRecoveryTokenValidator passwordRecoveryTokenValidator,
        SessionUser sessionUser, 
        VueSessionUser vueSessionUser,
        UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        ISession session) :base(sessionUser)
    {
        _passwordRecoveryTokenValidator = passwordRecoveryTokenValidator;
        _vueSessionUser = vueSessionUser;
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _session = session;
    }

    private RequestResult ValidateToken(string token)
    {
        var passwordToken = _passwordRecoveryTokenValidator.Run(token);
        var result = new RequestResult
        {
            success = true
        };

        if (passwordToken == null)
        {
            result.success = false;
            result.messageKey = FrontendMessageKeys.Error.User.PasswordResetTokenIsInvalid;
        }
        else if ((DateTime.Now - passwordToken.DateCreated).TotalDays > 3)
        {
            result.success = false;
            result.messageKey = FrontendMessageKeys.Error.User.PasswordResetTokenIsExpired;
        }

        return result;
    }

    [HttpGet]
    public JsonResult Validate([FromRoute] string token)
    {
        return Json(ValidateToken(token));
    }

    public readonly record struct SetNewPasswordJson(string token, string password);
    [HttpPost]
    public JsonResult SetNewPassword([FromBody] SetNewPasswordJson json)
    {
        var validationResult = ValidateToken(json.token);
        if (validationResult.success == false)
        {
            return Json(validationResult);
        }
        var result = PasswordResetPrepare.Run(json.token, _session);
        if (json.password.Trim().Length < 5)
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.PasswordTooShort
            });
        }


       
        var user = _userReadingRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(json.password.Trim(), user);
        _userWritingRepo.Update(user);

        _sessionUser.Login(user);
        return Json(new RequestResult
        {
            success = true,
            data = _vueSessionUser.GetCurrentUserData()
        });
    }
}

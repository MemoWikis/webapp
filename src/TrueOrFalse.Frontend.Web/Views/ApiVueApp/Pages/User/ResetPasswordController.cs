﻿
using System;
using System.Web.Mvc;

namespace VueApp;

public class ResetPasswordController : BaseController
{
    private readonly PasswordRecoveryTokenValidator _passwordRecoveryTokenValidator;
    public ResetPasswordController(PasswordRecoveryTokenValidator passwordRecoveryTokenValidator)
    {
        _passwordRecoveryTokenValidator = passwordRecoveryTokenValidator;
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
    public JsonResult Validate(string token)
    {
        return Json(ValidateToken(token), JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult SetNewPassword(string token, string newPassword)
    {
        var validationResult = ValidateToken(token);
        if (validationResult.success == false)
        {
            return Json(validationResult);
        }
        var result = PasswordResetPrepare.Run(token);

        var userRepo = Sl.Resolve<UserRepo>();
        var user = userRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(newPassword, user);
        userRepo.Update(user);

        SessionUser.Login(user);
        return Json(new RequestResult
        {
            success = true,
            data = VueSessionUser.GetCurrentUserData()
        });
    }
}

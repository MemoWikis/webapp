
using System;
using System.Web.Mvc;
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
        var result = PasswordResetPrepare.Run(token, _session);

       
        var user = _userReadingRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(newPassword, user);
        _userWritingRepo.Update(user);

        _sessionUser.Login(user);
        return Json(new RequestResult
        {
            success = true,
            data = _vueSessionUser.GetCurrentUserData()
        });
    }
}

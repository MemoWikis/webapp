
using System;
using System.Web.Mvc;

namespace VueApp;

public class VerifyEmailController : BaseController
{
    private readonly VueSessionUser _vueSessionUser;

    public VerifyEmailController(SessionUser sessionUser, VueSessionUser vueSessionUser) :base(sessionUser)
    {
        _vueSessionUser = vueSessionUser;
    }


    //[HttpPost]
    //public JsonResult VerifyEmail(string token)
    //{
    //    var validator = new ValidateEmailConfirmationKey(Sl.UserRepo);
    //    var mailConfirmed = validator.IsValid(emailKey);

    //    return Json(ValidateToken(token), JsonRequestBehavior.AllowGet);
    //}


}

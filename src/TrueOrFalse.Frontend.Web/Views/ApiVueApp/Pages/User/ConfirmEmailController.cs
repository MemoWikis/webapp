
using System;
using System.Web.Mvc;

namespace VueApp;

public class ConfirmEmailController : BaseController
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly UserRepo _userRepo;

    public ConfirmEmailController(SessionUser sessionUser, VueSessionUser vueSessionUser, UserRepo userRepo) :base(sessionUser)
    {
        _vueSessionUser = vueSessionUser;
        _userRepo = userRepo;
    }


    [HttpPost]
    public bool VerifyEmail(string token)
    {
        var validator = new EmailConfirmationService(_userRepo);
        var mailConfirmed = validator.ConfirmUserEmailByKey(token);

        return mailConfirmed;
    }


}

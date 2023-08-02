
using System.Web.Mvc;

namespace VueApp;

public class ConfirmEmailController : BaseController
{
    private readonly UserRepo _userRepo;

    public ConfirmEmailController(SessionUser sessionUser, UserRepo userRepo) :base(sessionUser)
    {
        _userRepo = userRepo;
    }


    [HttpPost]
    public JsonResult Run(string token)
    {
        var validator = new EmailConfirmationService(_userRepo);
        var mailConfirmed = validator.ConfirmUserEmailByKey(token);

        return Json(mailConfirmed);
    }
}

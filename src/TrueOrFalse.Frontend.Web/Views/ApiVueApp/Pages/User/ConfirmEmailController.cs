
using System.Web.Mvc;

namespace VueApp;

public class ConfirmEmailController : BaseController
{
    private readonly UserRepo _userRepo;
    private readonly EmailConfirmationService _emailConfirmationService;

    public ConfirmEmailController(SessionUser sessionUser, UserRepo userRepo, EmailConfirmationService emailConfirmationService) :base(sessionUser)
    {
        _userRepo = userRepo;
        _emailConfirmationService = emailConfirmationService;
    }

    [HttpPost]
    public JsonResult Run(string token)
    {
        var mailConfirmed = _emailConfirmationService.TryConfirmEmail(token);

        return Json(mailConfirmed);
    }
}

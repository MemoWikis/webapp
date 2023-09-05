

using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ConfirmEmailController : BaseController
{
 
    private readonly EmailConfirmationService _emailConfirmationService;

    public ConfirmEmailController(SessionUser sessionUser, EmailConfirmationService emailConfirmationService) :base(sessionUser)
    {
        _emailConfirmationService = emailConfirmationService;
    }

    [HttpPost]
    public JsonResult Run(string token)
    {
        var mailConfirmed = _emailConfirmationService.TryConfirmEmail(token);

        return Json(mailConfirmed);
    }
}

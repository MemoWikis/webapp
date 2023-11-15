

using HelperClassesControllers;
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
    public JsonResult Run([FromBody] ConfirmEmailTokenJson json)
    {
        var mailConfirmed = _emailConfirmationService.TryConfirmEmail(json.token);

        return Json(mailConfirmed);
    }


}

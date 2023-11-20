using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ConfirmEmailController : BaseController
{
 
    private readonly EmailConfirmationService _emailConfirmationService;

    public ConfirmEmailController(SessionUser sessionUser, EmailConfirmationService emailConfirmationService) :base(sessionUser)
    {
        _emailConfirmationService = emailConfirmationService;
    }

    public readonly record struct ConfirmEmailRunJson(string token);
    [HttpPost]
    public JsonResult Run([FromBody] ConfirmEmailRunJson json)
    {
        return Json(_emailConfirmationService.TryConfirmEmail(json.token));
    }
}

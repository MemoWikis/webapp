using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ConfirmEmailController(EmailConfirmationService _emailConfirmationService) : Controller
{
    public readonly record struct ConfirmEmailRunJson(string token);

    [HttpPost]
    public bool Run([FromBody] ConfirmEmailRunJson json)
    {
        return _emailConfirmationService.TryConfirmEmail(json.token);
    }
}
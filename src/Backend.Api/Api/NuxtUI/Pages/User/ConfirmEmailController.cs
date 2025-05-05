public class ConfirmEmailController(EmailConfirmationService _emailConfirmationService) : ApiBaseController
{
    public readonly record struct ConfirmEmailRunJson(string token);

    [HttpPost]
    public bool Run([FromBody] ConfirmEmailRunJson json)
    {
        return _emailConfirmationService.TryConfirmEmail(json.token);
    }
}
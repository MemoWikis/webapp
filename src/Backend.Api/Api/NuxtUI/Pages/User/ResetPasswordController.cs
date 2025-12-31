using ISession = NHibernate.ISession;

public class ResetPasswordController(
    PasswordRecoveryTokenValidator _passwordRecoveryTokenValidator,
    SessionUser _sessionUser,
    FrontEndUserData _frontEndUserData,
    UserReadingRepo _userReadingRepo,
    UserWritingRepo _userWritingRepo,
    PageViewRepo _pageViewRepo,
    ISession _session) : ApiBaseController
{
    public record struct ValidateTokenResult(bool Success, string MessageKey);

    private ValidateTokenResult ValidateToken(string token)
    {
        var passwordToken = _passwordRecoveryTokenValidator.Run(token);
        var result = new ValidateTokenResult
        {
            Success = true
        };

        if (passwordToken == null)
        {
            result.Success = false;
            result.MessageKey = FrontendMessageKeys.Error.User.PasswordResetTokenIsInvalid;
        }
        else if ((DateTime.Now - passwordToken.DateCreated).TotalDays > 3)
        {
            result.Success = false;
            result.MessageKey = FrontendMessageKeys.Error.User.PasswordResetTokenIsExpired;
        }

        return result;
    }

    public readonly record struct ValidateResult(bool Success, string MessageKey);

    [HttpGet]
    public ValidateResult Validate([FromRoute] string id)
    {
        var validateToken = ValidateToken(id);
        return new ValidateResult
        {
            Success = validateToken.Success, MessageKey = validateToken.MessageKey
        };
    }

    public readonly record struct SetNewPasswordJson(string token, string password);

    public readonly record struct SetNewPasswordResult(
        bool Success,
        FrontEndUserData.CurrentUserData Data,
        string MessageKey);

    [HttpPost]
    public SetNewPasswordResult SetNewPassword([FromBody] SetNewPasswordJson json)
    {
        var validationResult = ValidateToken(json.token);
        if (validationResult.Success == false)
        {
            return new SetNewPasswordResult
            {
                Success = validationResult.Success, MessageKey = validationResult.MessageKey
            };
        }

        var result = PasswordResetPrepare.Run(json.token, _session);

        if (json.password.Trim().Length < 5)
        {
            return new SetNewPasswordResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.PasswordTooShort
            };
        }

        var user = _userReadingRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(json.password.Trim(), user);
        _userWritingRepo.Update(user);

        _sessionUser.Login(user, _pageViewRepo);
        return new SetNewPasswordResult
        {
            Success = true,
            Data = _frontEndUserData.Get()
        };
    }
}
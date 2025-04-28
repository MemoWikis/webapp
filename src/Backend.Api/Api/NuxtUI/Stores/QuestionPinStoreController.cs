public class QuestionPinStoreController(
    SessionUser _sessionUser,
    QuestionInKnowledge _questionInKnowledge,
    Logg _logg,
    ExtendedUserCache _extendedUserCache) : ApiBaseController
{
    public readonly record struct PinResult(bool Success, string MessageKey);

    [HttpPost]
    public PinResult Pin([FromRoute] int id)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new PinResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        var limitcheck = new LimitCheck(_logg, _sessionUser);
        if (!limitcheck.CanAddNewKnowledge(logExceedance: true))
        {
            return new PinResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Subscription.CantAddKnowledge
            };
        }

        try
        {
            _questionInKnowledge.Pin(id, _sessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r.Error(e,
                $"Error while pinning question Id={id} for userId={_sessionUser.UserId}");
            return new PinResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.Default };
        }

        var success = EntityCache.GetQuestion(id)
            .IsInWishknowledge(_sessionUser.UserId, _extendedUserCache);
        return new PinResult
            { Success = success, MessageKey = success ? null : FrontendMessageKeys.Error.Default };
    }

    [HttpPost]
    public PinResult Unpin([FromRoute] int id)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new PinResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        try
        {
            _questionInKnowledge.Unpin(id, _sessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r.Error(e,
                $"Error while unpinning question Id={id} for userId={_sessionUser.UserId}");
            return new PinResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.Default };
        }

        var success = !EntityCache.GetQuestion(id)
            .IsInWishknowledge(_sessionUser.UserId, _extendedUserCache);
        return new PinResult
            { Success = success, MessageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
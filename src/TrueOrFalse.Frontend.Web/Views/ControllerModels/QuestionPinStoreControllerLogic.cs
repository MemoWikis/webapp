using System;
using Serilog;
using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    public dynamic Pin(int id)
    {
        if (!SessionUser.IsLoggedIn)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        if (!LimitCheck.CanAddNewKnowledge())
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Subscription.CantAddKnowledge };
        }

        try
        {
            QuestionInKnowledge.Pin(id, SessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r().Error(e, $"Error while pinning question id={id} for userId={SessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = EntityCache.GetQuestion(id).IsInWishknowledge();
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }

    public dynamic Unpin(int id)
    {
        if (!SessionUser.IsLoggedIn)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        try
        {
            QuestionInKnowledge.Unpin(id, SessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r().Error(e, $"Error while unpinning question id={id} for userId={SessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = !EntityCache.GetQuestion(id).IsInWishknowledge();
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
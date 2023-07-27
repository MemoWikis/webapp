using System;
using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic
{
    private readonly QuestionInKnowledge _questionInKnowledge;

    public QuestionPinStoreControllerLogic(QuestionInKnowledge questionInKnowledge)
    {
        _questionInKnowledge = questionInKnowledge;
    }
    public dynamic Pin(int id, SessionUser sessionUser)
    {
        if (!sessionUser.IsLoggedIn)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        if (!LimitCheck.CanAddNewKnowledge(sessionUser))
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Subscription.CantAddKnowledge };
        }

        try
        {
            _questionInKnowledge.Pin(id, sessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r().Error(e, $"Error while pinning question id={id} for userId={sessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId);
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }

    public dynamic Unpin(int id, SessionUser sessionUser)
    {
        if (!sessionUser.IsLoggedIn)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        try
        {
            _questionInKnowledge.Unpin(id, sessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r().Error(e, $"Error while unpinning question id={id} for userId={sessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = !EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId);
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
﻿using System;

public class QuestionPinStoreControllerLogic :IRegisterAsInstancePerLifetime
{
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly SessionUserCache _sessionUserCache;
    private readonly Logg _logg;

    public QuestionPinStoreControllerLogic(
        QuestionInKnowledge questionInKnowledge,
        SessionUserCache sessionUserCache,
        Logg logg)
    {
        _questionInKnowledge = questionInKnowledge;
        _sessionUserCache = sessionUserCache;
        _logg = logg;
    }
    public dynamic Pin(int id, SessionUser sessionUser)
    {
        if (!sessionUser.IsLoggedIn)
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        var limitcheck = new LimitCheck(_logg, sessionUser); 
        if (!limitcheck.CanAddNewKnowledge(logExceedance: true))
        {
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Subscription.CantAddKnowledge };
        }

        try
        {
            _questionInKnowledge.Pin(id, sessionUser.UserId);
        }
        catch (Exception e)
        {
            Logg.r.Error(e, $"Error while pinning question id={id} for userId={sessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId, _sessionUserCache);
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
            Logg.r.Error(e, $"Error while unpinning question id={id} for userId={sessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = !EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId, _sessionUserCache);
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
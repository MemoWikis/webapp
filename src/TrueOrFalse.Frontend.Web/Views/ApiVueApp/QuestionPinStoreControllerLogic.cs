using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic :IRegisterAsInstancePerLifetime
{
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SessionUserCache _sessionUserCache;

    public QuestionPinStoreControllerLogic(QuestionInKnowledge questionInKnowledge,
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor,
        SessionUserCache sessionUserCache)
    {
        _questionInKnowledge = questionInKnowledge;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _sessionUserCache = sessionUserCache;
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
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, $"Error while pinning question id={id} for userId={sessionUser.UserId}");
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
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, $"Error while unpinning question id={id} for userId={sessionUser.UserId}");
            return new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default };
        }

        var success = !EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId, _sessionUserCache);
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
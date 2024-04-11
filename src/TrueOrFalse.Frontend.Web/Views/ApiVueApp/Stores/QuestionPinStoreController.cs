using System;
using Microsoft.AspNetCore.Mvc;

public class QuestionPinStoreControllerQuestionPinStoreController(
    SessionUser _sessionUser,
    QuestionInKnowledge _questionInKnowledge,
    Logg _logg,
    SessionUserCache _sessionUserCache) : Controller
{
    public readonly record struct QuestionPinStoreResult(bool Success, string MessageKey);

    [HttpPost]
    public QuestionPinStoreResult Pin([FromRoute] int id)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new QuestionPinStoreResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.User.NotLoggedIn };
        }

        var limitcheck = new LimitCheck(_logg, _sessionUser);
        if (!limitcheck.CanAddNewKnowledge(logExceedance: true))
        {
            return new QuestionPinStoreResult
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
            return new QuestionPinStoreResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.Default };
        }

        var success = EntityCache.GetQuestion(id)
            .IsInWishknowledge(_sessionUser.UserId, _sessionUserCache);
        return new QuestionPinStoreResult
            { Success = success, MessageKey = success ? null : FrontendMessageKeys.Error.Default };
    }

    [HttpPost]
    public QuestionPinStoreResult Unpin([FromRoute] int id)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new QuestionPinStoreResult
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
            return new QuestionPinStoreResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.Default };
        }

        var success = !EntityCache.GetQuestion(id)
            .IsInWishknowledge(_sessionUser.UserId, _sessionUserCache);
        return new QuestionPinStoreResult
            { Success = success, MessageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
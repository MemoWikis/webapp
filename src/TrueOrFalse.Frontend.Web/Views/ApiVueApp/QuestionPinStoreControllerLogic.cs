using System;
using TrueOrFalse.Domain;

public class QuestionPinStoreControllerLogic :IRegisterAsInstancePerLifetime
{
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public QuestionPinStoreControllerLogic(QuestionInKnowledge questionInKnowledge,
        CategoryValuationReadingRepo categoryValuationReadingRepo, 
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        _questionInKnowledge = questionInKnowledge;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
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

        var success = EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId,_categoryValuationReadingRepo, _userReadingRepo,_questionValuationRepo  );
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

        var success = !EntityCache.GetQuestion(id).IsInWishknowledge(sessionUser.UserId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo);
        return new RequestResult { success = success, messageKey = success ? null : FrontendMessageKeys.Error.Default };
    }
}
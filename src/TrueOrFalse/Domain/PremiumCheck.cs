using System;
using System.Linq;

namespace TrueOrFalse.Domain;

public class PremiumCheck
{
    private static readonly int _privateQuestionsQuantity = 20;
    private static readonly int _privateTopicsQuantity = 10;
    private static readonly int _wishCountKnowledge = 50;

    public static bool CanAddNewKnowledge()
    {
        var result = SessionUser.User.SubscriptionDuration != null &&
                     SessionUser.User.SubscriptionDuration > DateTime.Now ||
                     SessionUser.User.WishCountQuestions < _wishCountKnowledge;
        return result ;
    }

    public static bool CanSavePrivateQuestion()
    {
        return SessionUser.User.SubscriptionDuration != null &&
                SessionUser.User.SubscriptionDuration > DateTime.Now ||
               EntityCache.GetPrivateCategoryIdsFromUser(SessionUser.UserId).Count() < _privateQuestionsQuantity;
    }

    public static bool CanSavePrivateTopic()
    {
        return SessionUser.User.SubscriptionDuration != null &&
                SessionUser.User.SubscriptionDuration > DateTime.Now ||
               EntityCache.GetPrivateCategoryIdsFromUser(SessionUser.UserId).Count() < _privateTopicsQuantity;
    }
}
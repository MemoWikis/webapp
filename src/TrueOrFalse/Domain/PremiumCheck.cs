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
        return SessionUser.User.SubscriptionDuration != null &&
                     SessionUser.User.SubscriptionDuration > DateTime.Now ||
                     SessionUser.User.WishCountQuestions < _wishCountKnowledge || SessionUser.IsInstallationAdmin;
    }

    public static bool CanSavePrivateQuestion()
    {
        return SessionUser.User.SubscriptionDuration != null &&
                SessionUser.User.SubscriptionDuration > DateTime.Now ||
               EntityCache.GetPrivateCategoryIdsFromUser(SessionUser.UserId).Count() < _privateQuestionsQuantity || SessionUser.IsInstallationAdmin;
    }

    public static bool CanSavePrivateTopic()
    {
        return SessionUser.User.SubscriptionDuration != null &&
                SessionUser.User.SubscriptionDuration > DateTime.Now ||
               EntityCache.GetPrivateCategoryIdsFromUser(SessionUser.UserId).Count() < _privateTopicsQuantity || SessionUser.IsInstallationAdmin;
    }
}
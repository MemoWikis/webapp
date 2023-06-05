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
        return SessionUserLegacy.IsInstallationAdmin ||
               HasActiveSubscriptionPlan() ||
               SessionUserLegacy.User.WishCountQuestions < _wishCountKnowledge;
      }

    public static bool CanSavePrivateQuestion()
    {
        return SessionUserLegacy.IsInstallationAdmin ||
               HasActiveSubscriptionPlan() ||
               EntityCache.GetPrivateQuestionIdsFromUser(SessionUserLegacy.UserId).Count() < _privateQuestionsQuantity;
    }

    public static bool CanSavePrivateTopic()
    {
        return SessionUserLegacy.IsInstallationAdmin ||
               HasActiveSubscriptionPlan() ||
               EntityCache.GetPrivateCategoryIdsFromUser(SessionUserLegacy.UserId).Count() < _privateTopicsQuantity;
    }

    private static bool HasActiveSubscriptionPlan()
    {
        return SessionUserLegacy.User.EndDate != null && SessionUserLegacy.User.EndDate > DateTime.Now;
    }
}
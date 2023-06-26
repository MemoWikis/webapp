using System.Linq;

namespace TrueOrFalse.Domain;

public class PremiumCheck
{
    private static readonly int _privateQuestionsQuantity = 20;
    private static readonly int _privateTopicsQuantity = 10;
    private static readonly int _wishCountKnowledge = 50;

    public static bool CanAddNewKnowledge(SessionUser sessionUser)
    {
        return sessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan(sessionUser) ||
               sessionUser.User.WishCountQuestions < _wishCountKnowledge;
      }

    public static bool CanSavePrivateQuestion(SessionUser sessionUser)
    {
        return sessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan(sessionUser) ||
               EntityCache.GetPrivateQuestionIdsFromUser(sessionUser.UserId).Count() < _privateQuestionsQuantity;
    }

    public static bool CanSavePrivateTopic(SessionUser sessionUser)
    {
        return sessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan(sessionUser) ||
               EntityCache.GetPrivateCategoryIdsFromUser(sessionUser.UserId).Count() < _privateTopicsQuantity;
    }

    private static bool HasActiveSubscriptionPlan(SessionUser sessionUser)
    {
        return sessionUser.User.EndDate != null && sessionUser.User.EndDate > DateTime.Now;
    }
}
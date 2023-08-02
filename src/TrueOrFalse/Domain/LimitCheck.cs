using System.Linq;

namespace TrueOrFalse.Domain;

public class LimitCheck
{
    private static readonly int _privateQuestionsQuantity = 20;
    private static readonly int _privateTopicsQuantity = 10;
    private static readonly int _wishCountKnowledge = 50;

    public static dynamic GetBasicLimits()
    {
        return new
        {
            maxPrivateTopicCount = _privateTopicsQuantity,
            maxPrivateQuestionCount = _privateQuestionsQuantity,
            maxWishknowledgeCount = _wishCountKnowledge,
            testToBeDeleted = true
        };
    }

    public static bool CanAddNewKnowledge(SessionUser sessionUser, bool logExceedance = false)
    {
        if (sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan(sessionUser))

            return true;
        
        var limitExceeded = sessionUser.User.WishCountQuestions < _wishCountKnowledge;

        if (limitExceeded && logExceedance)
        {
            LogExceededLimit("question in wishknowledge");
        }

        return limitExceeded;
    }

    public static bool CanSavePrivateQuestion(SessionUser sessionUser, bool logExceedance = false)
    {
        if (sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan(sessionUser))

            return true;

        var limitExceeded = EntityCache.GetPrivateQuestionIdsFromUser(sessionUser.UserId).Count() < _privateQuestionsQuantity;

        if (limitExceeded && logExceedance)
        {
            LogExceededLimit("private questions");
        }

        return limitExceeded;

    }

    public static bool CanSavePrivateTopic(SessionUser sessionUser,  bool logExceedance = false)
    {
        if (sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan(sessionUser))

            return true;
        
        var limitExceeded = EntityCache.GetPrivateCategoryIdsFromUser(sessionUser.UserId).Count() < _privateTopicsQuantity;

        if (limitExceeded && logExceedance)
        {
            LogExceededLimit("private topics");
        }

        return limitExceeded;
    }

    private static bool HasActiveSubscriptionPlan(SessionUser sessionUser)
    {
        return sessionUser.User.EndDate != null && sessionUser.User.EndDate > DateTime.Now;
    }

    public static void LogExceededLimit(string type)
    {
        Logg.r().Information("LimitCheck: max. number of type {type} exceeded", type);
    }
}
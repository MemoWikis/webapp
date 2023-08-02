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
        
        var withinLimit = sessionUser.User.WishCountQuestions < _wishCountKnowledge;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("question in wishknowledge");
        }

        return withinLimit;
    }

    public static bool CanSavePrivateQuestion(SessionUser sessionUser, bool logExceedance = false)
    {
        if (sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan(sessionUser))

            return true;

        var withinLimit = EntityCache.GetPrivateQuestionIdsFromUser(sessionUser.UserId).Count() < _privateQuestionsQuantity;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("private questions");
        }

        return withinLimit;

    }

    public static bool CanSavePrivateTopic(SessionUser sessionUser,  bool logExceedance = false)
    {
        if (sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan(sessionUser))

            return true;
        
        var withinLimit = EntityCache.GetPrivateCategoryIdsFromUser(sessionUser.UserId).Count() < _privateTopicsQuantity;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("private topics");
        }

        return withinLimit;
    }

    private static bool HasActiveSubscriptionPlan(SessionUser sessionUser)
    {
        return sessionUser.User.EndDate != null && sessionUser.User.EndDate > DateTime.Now;
    }

    public static void LogExceededLimit(string type)
    {
        Logg.r().Information("LimitCheck: max. number of type '{type}' exceeded", type);
    }
}
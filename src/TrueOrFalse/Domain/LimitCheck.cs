using System;
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
            maxWishknowledgeCount = _wishCountKnowledge
        };
    }

    public static bool CanAddNewKnowledge()
    {
        return SessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan() ||
               SessionUser.User.WishCountQuestions < _wishCountKnowledge;
      }

    public static bool CanSavePrivateQuestion()
    {
        return SessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan() ||
               EntityCache.GetPrivateQuestionIdsFromUser(SessionUser.UserId).Count() < _privateQuestionsQuantity;
    }

    public static bool CanSavePrivateTopic()
    {
        return SessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan() ||
               EntityCache.GetPrivateCategoryIdsFromUser(SessionUser.UserId).Count() < _privateTopicsQuantity;
    }

    private static bool HasActiveSubscriptionPlan()
    {
        return SessionUser.User.EndDate != null && SessionUser.User.EndDate > DateTime.Now;
    }
}
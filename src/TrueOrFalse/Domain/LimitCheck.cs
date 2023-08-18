using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

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

    public static bool CanAddNewKnowledge(SessionUser sessionUser)
    {
        return sessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan(sessionUser) ||
               sessionUser.User.WishCountQuestions < _wishCountKnowledge;
    }

    public static bool CanSavePrivateQuestion(SessionUser sessionUser, 
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment)
    {
        return sessionUser.IsInstallationAdmin ||
               HasActiveSubscriptionPlan(sessionUser) ||
               EntityCache.GetPrivateQuestionIdsFromUser(sessionUser.UserId,
                   httpContextAccessor, 
                   webHostEnvironment)
                   .Count() < _privateQuestionsQuantity;
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
﻿public class LimitCheck(Logg _logg, SessionUser _sessionUser)
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

    public bool CanAddNewKnowledge(bool logExceedance = false)
    {
        if (_sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan())

            return true;

        var withinLimit = _sessionUser.User.WishCountQuestions < _wishCountKnowledge;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("question in wishknowledge", _logg);
        }

        return withinLimit;
    }

    public bool CanSavePrivateQuestion(bool logExceedance = false)
    {
        if (_sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan())
            return true;

        var withinLimit = EntityCache.GetPrivateQuestionIdsFromUser(_sessionUser.UserId).Count() <
                          _privateQuestionsQuantity;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("private questions", _logg);
        }

        return withinLimit;
    }

    public bool CanSavePrivateTopic(bool logExceedance = false)
    {
        if (_sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan())

            return true;

        var withinLimit = EntityCache.GetPrivateCategoryIdsFromUser(_sessionUser.UserId).Count() <
                          _privateTopicsQuantity;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("private topics", _logg);
        }

        return withinLimit;
    }

    private bool HasActiveSubscriptionPlan()
    {
        return _sessionUser.User.EndDate != null && _sessionUser.User.EndDate > DateTime.Now;
    }

    public static void LogExceededLimit(string type, Logg logg)
    {
        Logg.r.Information("LimitCheck: max. number of type '{type}' exceeded", type);
    }
}
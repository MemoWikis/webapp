public class LimitCheck
{
    private readonly Logg _logg;
    private readonly SessionUser _sessionUser;
    private static readonly int _privateQuestionsQuantity = 20;
    private static readonly int _privateTopicsQuantity = 10;
    private static readonly int _wishCountKnowledge = 50;

    public LimitCheck(Logg logg, SessionUser sessionUser)
    {
        _logg = logg;
        _sessionUser = sessionUser;
    }

    public readonly record struct BasicLimits(
        int MaxPrivateTopicCount,
        int MaxPrivateQuestionCount,
        int MaxWishknowledgeCount,
        bool TestToBeDeleted = true);

    public static BasicLimits GetBasicLimits()
    {
        return new BasicLimits
        {
            MaxPrivateTopicCount = _privateTopicsQuantity,
            MaxPrivateQuestionCount = _privateQuestionsQuantity,
            MaxWishknowledgeCount = _wishCountKnowledge,
            TestToBeDeleted = true
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

    
    public bool VerifyUserAccessDuration(int count)
    {
        var accountAgeInDays = (DateTime.Now - _sessionUser.AccountCreated).TotalDays;
        var lastEdit = (DateTime.Now - _sessionUser.LastEdit).TotalHours;  
        if (accountAgeInDays < 7 || _sessionUser.LastEdit < 24)
            return count <= 2;

        if (accountAgeInDays < 182)
            return count <= 5; 
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
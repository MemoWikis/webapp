public class LimitCheck(SessionUser _sessionUser)
{
    private static readonly int _privateQuestionsQuantity = 20;
    private static readonly int _privatePagesQuantity = 10;
    private static readonly int _wishCountKnowledge = 50;

    public readonly record struct BasicLimits(
        int MaxPrivatePageCount,
        int MaxPrivateQuestionCount,
        int MaxWishKnowledgeCount,
        bool TestToBeDeleted = true);

    public static BasicLimits GetBasicLimits()
    {
        return new BasicLimits
        {
            MaxPrivatePageCount = _privatePagesQuantity,
            MaxPrivateQuestionCount = _privateQuestionsQuantity,
            MaxWishKnowledgeCount = _wishCountKnowledge,
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
            LogExceededLimit("question in wishKnowledge");
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
            LogExceededLimit("private questions");
        }

        return withinLimit;
    }

    public bool CanSavePrivatePage(bool logExceedance = false)
    {
        if (_sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan())
            return true;

        var withinLimit = EntityCache.GetPrivatePageIdsFromUser(_sessionUser.UserId).Count() < _privatePagesQuantity;

        if (!withinLimit && logExceedance)
        {
            LogExceededLimit("private pages");
        }

        return withinLimit;
    }

    public bool NewPrivateQuestionsWillExceedLimit(int count)
    {
        if (_sessionUser.IsInstallationAdmin || HasActiveSubscriptionPlan())
            return false;

        return EntityCache.GetPrivateQuestionIdsFromUser(_sessionUser.UserId).Count() + count > _privateQuestionsQuantity;
    }

    public bool CanCreatePrivateWiki(bool logExceedance = false) => CanSavePrivatePage(logExceedance);

    private bool HasActiveSubscriptionPlan()
    {
        return _sessionUser.User.EndDate != null && _sessionUser.User.EndDate > DateTime.Now;
    }

    public static void LogExceededLimit(string type)
    {
        Log.Information("LimitCheck: max. number of type '{type}' exceeded", type);
    }
}
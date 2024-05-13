using TrueOrFalse.Utilities.ScheduledJobs;

public class QuestionDelete : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly ExtendedUserCache _extendedUserCache;

    public QuestionDelete(
        PermissionCheck permissionCheck,
        SessionUser sessionUser,
        QuestionReadingRepo questionReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        ExtendedUserCache extendedUserCache)
    {
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
        _questionReadingRepo = questionReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _extendedUserCache = extendedUserCache;
    }

    public void Run(int questionId)
    {
        var question = _questionReadingRepo.GetById(questionId);
        var questionCacheItem = EntityCache.GetQuestion(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(_sessionUser);

        var canBeDeletedResult = CanBeDeleted(_sessionUser.UserId, question);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: Question is " +
                                canBeDeletedResult.WuwiCount + "x in Wishknowledge");
        }

        EntityCache.Remove(questionCacheItem);
        _extendedUserCache.RemoveQuestionForAllUsers(questionId);

        JobScheduler.StartImmediately_DeleteQuestion(questionId, _sessionUser.UserId);
    }

    public CanBeDeletedResult CanBeDeleted(int currentUserId, Question question)
    {
        if (_permissionCheck.CanDelete(question))
        {
            if (!_sessionUser.IsInstallationAdmin)
            {
                var howOftenInOtherPeopleWuwi =
                    _questionValuationReadingRepo.HowOftenInOtherPeoplesWuwi(currentUserId,
                        question.Id);
                if (howOftenInOtherPeopleWuwi > 0)
                {
                    return new CanBeDeletedResult
                    {
                        Yes = false,
                        WuwiCount = howOftenInOtherPeopleWuwi
                    };
                }
            }

            return new CanBeDeletedResult { Yes = true };
        }

        return new CanBeDeletedResult
        {
            Yes = false,
            HasRights = false
        };
    }

    public class CanBeDeletedResult
    {
        public bool Yes;
        public int WuwiCount;
        public bool HasRights = true;
    }
}
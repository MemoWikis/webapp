using TrueOrFalse.Utilities.ScheduledJobs;

public class QuestionDelete : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly UserRepo _userRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public QuestionDelete(PermissionCheck permissionCheck,
        SessionUser sessionUser,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        QuestionReadingRepo questionReadingRepo,
        UserRepo userRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _questionReadingRepo = questionReadingRepo;
        _userRepo = userRepo;
        _questionValuationRepo = questionValuationRepo;
    }
    public void Run(int questionId)
    {
        
        var question = _questionReadingRepo.GetById(questionId);
        var questionCacheItem = EntityCache.GetQuestion(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(_sessionUser);

        var canBeDeletedResult = CanBeDeleted(_sessionUser.UserId, question);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: Question is " + canBeDeletedResult.WuwiCount + "x in Wishknowledge");
        }

        EntityCache.Remove(questionCacheItem);
        SessionUserCache.RemoveQuestionValuationForUser(_sessionUser.UserId, questionId, _categoryValuationReadingRepo, _userRepo, _questionValuationRepo);
        JobScheduler.StartImmediately_DeleteQuestion(questionId);
    }

    public CanBeDeletedResult CanBeDeleted(int currentUserId, Question question)
    {
        var questionCreator = question.Creator;
        if (_permissionCheck.CanDelete(question))
        {
            var howOftenInOtherPeopleWuwi = _questionValuationRepo.HowOftenInOtherPeoplesWuwi(currentUserId, question.Id);
            if (howOftenInOtherPeopleWuwi > 0)
            {
                return new CanBeDeletedResult
                {
                    Yes = false,
                    WuwiCount = howOftenInOtherPeopleWuwi
                };
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

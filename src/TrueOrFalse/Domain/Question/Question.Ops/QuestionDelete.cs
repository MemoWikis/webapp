using TrueOrFalse.Utilities.ScheduledJobs;

public class QuestionDelete : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;

    public QuestionDelete(PermissionCheck permissionCheck )
    {
        _permissionCheck = permissionCheck;
    }
    public void Run(int questionId)
    {
        var questionRepo = Sl.QuestionRepo;
        var question = questionRepo.GetById(questionId);
        var questionCacheItem = EntityCache.GetQuestion(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(question.Creator?.Id ?? -1);

        var canBeDeletedResult = CanBeDeleted(SessionUserLegacy.UserId, question);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: Question is " + canBeDeletedResult.WuwiCount + "x in Wishknowledge");
        }

        EntityCache.Remove(questionCacheItem);
        SessionUserCache.RemoveQuestionValuationForUser(SessionUserLegacy.UserId, questionId);
        JobScheduler.StartImmediately_DeleteQuestion(questionId);
    }

    public CanBeDeletedResult CanBeDeleted(int currentUserId, Question question)
    {
        var questionCreator = question.Creator;
        if (_permissionCheck.CanDelete(question))
        {
            var howOftenInOtherPeopleWuwi = Sl.R<QuestionRepo>().HowOftenInOtherPeoplesWuwi(currentUserId, question.Id);
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

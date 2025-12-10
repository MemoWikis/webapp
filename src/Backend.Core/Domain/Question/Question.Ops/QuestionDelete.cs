public class QuestionDelete(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    QuestionReadingRepo _questionReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public void Run(int questionId)
    {
        var question = _questionReadingRepo.GetById(questionId);
        var questionCacheItem = EntityCache.GetQuestion(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(_sessionUser);

        var canBeDeletedResult = CanBeDeleted(_sessionUser.UserId, question);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: Question is " +
                                canBeDeletedResult.WishKnowledgeCount + "x in WishKnowledge");
        }
        var parentIds = questionCacheItem.Pages.Select(c => c.Id).ToList();

        EntityCache.Remove(questionCacheItem);
        _extendedUserCache.RemoveQuestionForAllUsers(questionId);

        var deleteImage = new DeleteImage();
        deleteImage.RemoveAllForQuestion(questionId);

        var parentIdsString = string.Join("-", parentIds);

        JobScheduler.StartImmediately_DeleteQuestion(questionId, _sessionUser.UserId, parentIdsString);
    }

    public CanBeDeletedResult CanBeDeleted(int currentUserId, Question question)
    {
        if (_permissionCheck.CanDelete(question))
        {
            if (!_sessionUser.IsInstallationAdmin)
            {
                var howOftenInOtherPeopleWishKnowledge =
                    _questionValuationReadingRepo.HowOftenInOtherPeoplesWishKnowledge(currentUserId,
                        question.Id);
                if (howOftenInOtherPeopleWishKnowledge > 0)
                {
                    return new CanBeDeletedResult
                    {
                        Yes = false,
                        WishKnowledgeCount = howOftenInOtherPeopleWishKnowledge
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
        public int WishKnowledgeCount;
        public bool HasRights = true;
    }
}
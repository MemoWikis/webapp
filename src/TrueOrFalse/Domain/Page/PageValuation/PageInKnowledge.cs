using FluentNHibernate.Conventions;

public class PageInKnowledge(
    QuestionInKnowledge _questionInKnowledge,
    UserReadingRepo _userReadingRepo,
    ExtendedUserCache _extendedUserCache)
    : IRegisterAsInstancePerLifetime
{
    private IList<int> QuestionsInValuatedCategories(
        int userId,
        IList<int> questionIds,
        int exeptCategoryId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var valuatedCategories = _extendedUserCache
            .GetPageValuations(userId)
            .Where(v => v.IsInWishKnowledge());

        if (exeptCategoryId != -1)
            valuatedCategories = valuatedCategories.Where(v => v.PageId != exeptCategoryId);

        var questionsInOtherValuatedCategories = valuatedCategories
            .SelectMany(v =>
            {
                var page = EntityCache.GetPage(v.PageId);

                return page == null
                    ? new List<QuestionCacheItem>()
                    : page.GetAggregatedQuestionsFromMemoryCache(userId);
            })
            .GetIds()
            .Distinct()
            .ToList();

        return questionsInOtherValuatedCategories;
    }

    public void UnpinQuestionsInCategoryInDatabase(int pageId, int userId)
    {
        var user = _userReadingRepo.GetByIds(userId).First();
        var questionsInCategory = EntityCache.GetPage(pageId)
            .GetAggregatedQuestionsFromMemoryCache(userId);
        var questionIds = questionsInCategory.GetIds();

        var questionsInPinnedCategories =
            QuestionsInValuatedCategories(user.Id, questionIds, exeptCategoryId: pageId);

        var questionInOtherPinnedEntitites = questionsInPinnedCategories;
        var questionsToUnpin = questionsInCategory
            .Where(question => questionInOtherPinnedEntitites.All(id => id != question.Id))
            .ToList();

        foreach (var question in questionsToUnpin)
            _questionInKnowledge.Unpin(question.Id, user.Id);

        _questionInKnowledge.UpdateTotalRelevancePersonalInCache(questionsToUnpin);
        _questionInKnowledge.SetUserWishCountQuestions(user.Id);
    }
}
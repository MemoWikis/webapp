using FluentNHibernate.Conventions;

public class PageInKnowledge(
    QuestionInKnowledge _questionInKnowledge,
    UserReadingRepo _userReadingRepo,
    ExtendedUserCache _extendedUserCache)
    : IRegisterAsInstancePerLifetime
{
    private IList<int> QuestionsInValuatedPages(
        int userId,
        IList<int> questionIds,
        int exceptPageId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var evaluatedPages = _extendedUserCache
            .GetPageValuations(userId)
            .Where(v => v.IsInWishKnowledge());

        if (exceptPageId != -1)
            evaluatedPages = evaluatedPages.Where(v => v.PageId != exceptPageId);

        var questionsInValuatedPages = evaluatedPages
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

        return questionsInValuatedPages;
    }

    public void UnpinQuestionsInPageInDatabase(int pageId, int userId)
    {
        var user = _userReadingRepo.GetByIds(userId).First();
        var questionsInPage = EntityCache.GetPage(pageId)
            .GetAggregatedQuestionsFromMemoryCache(userId);
        var questionIds = questionsInPage.GetIds();

        var questionsInValuatedPages = QuestionsInValuatedPages(user.Id, questionIds, exceptPageId: pageId);

        var questionInOtherPinnedEntities = questionsInValuatedPages;
        var questionsToUnpin = questionsInPage
            .Where(question => questionInOtherPinnedEntities.All(id => id != question.Id))
            .ToList();

        foreach (var question in questionsToUnpin)
            _questionInKnowledge.Unpin(question.Id, user.Id);

        _questionInKnowledge.UpdateTotalRelevancePersonalInCache(questionsToUnpin);
        _questionInKnowledge.SetUserWishCountQuestions(user.Id);
    }
}
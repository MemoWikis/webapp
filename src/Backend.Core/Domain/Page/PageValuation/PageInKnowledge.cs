using FluentNHibernate.Conventions;

public class PageInKnowledge(
    LoggedInUserCache _loggedInUserCache)
    : IRegisterAsInstancePerLifetime
{
    private IList<int> QuestionsInValuatedPages(
        int userId,
        IList<int> questionIds,
        int exceptPageId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        var evaluatedPages = _loggedInUserCache
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
                    : page.GetAggregatedQuestions(userId);
            })
            .GetIds()
            .Distinct()
            .ToList();

        return questionsInValuatedPages;
    }
}
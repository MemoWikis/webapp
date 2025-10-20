using FluentNHibernate.Conventions;

public class PageInKnowledge(
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
            .Where(v => v.IsInWishknowledge());

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
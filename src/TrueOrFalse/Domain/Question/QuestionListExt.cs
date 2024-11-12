using System.Diagnostics;

public static class QuestionListExt
{
    public static IList<int> GetIds(this IEnumerable<QuestionCacheItem> questions) =>
        questions.Select(q => q.Id).ToList();

    public static IEnumerable<PageCacheItem> GetAllPages(
        this IEnumerable<QuestionCacheItem> questions) =>
        questions.SelectMany(q =>
                EntityCache.GetPages(
                    q.Pages.Where(c => c != null)
                        .Select(c => c.Id)))
            .Distinct();

    public static IEnumerable<QuestionCacheItemInPage> QuestionsInPages(
        this IEnumerable<QuestionCacheItem> questions)
    {
        var questionsArray = questions as QuestionCacheItem[] ?? questions.ToArray();

        return questionsArray.GetAllPages()
            .Select(c => new QuestionCacheItemInPage
            {
                PageCacheItem = c,
                QuestionCacheItems = questionsArray.Where(q => q.Pages.Any(x => x.Id == c.Id))
                    .ToList()
            });
    }
}

[DebuggerDisplay("{PageCacheItem.Name} {Questions.Count}")]

public class QuestionCacheItemInPage
{
    public PageCacheItem PageCacheItem;
    public IList<QuestionCacheItem> QuestionCacheItems;
}
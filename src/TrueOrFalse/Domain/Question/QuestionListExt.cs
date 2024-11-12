using System.Diagnostics;

public static class QuestionListExt
{
    public static Question ById(this IEnumerable<Question> questions, int id) =>
        questions.FirstOrDefault(question => question.Id == id);

    public static IList<int> GetIds(this IEnumerable<QuestionCacheItem> questions) =>
        questions.Select(q => q.Id).ToList();

    public static IEnumerable<PageCacheItem> GetAllCategories(
        this IEnumerable<Question> questions) =>
        questions.SelectMany(q =>
                EntityCache.GetPages(
                    q.Pages.Where(c => c != null)
                        .Select(c => c.Id)))
            .Distinct();

    public static IEnumerable<PageCacheItem> GetAllCategories(
        this IEnumerable<QuestionCacheItem> questions) =>
        questions.SelectMany(q =>
                EntityCache.GetPages(
                    q.Pages.Where(c => c != null)
                        .Select(c => c.Id)))
            .Distinct();

    public static IEnumerable<QuestionsInPage> QuestionsInCategories(
        this IEnumerable<Question> questions)
    {
        var questionsArray = questions as Question[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionsInPage
            {
                PageCacheItem = c,
                Questions = questionsArray.Where(q => q.Pages.Any(x => x.Id == c.Id)).ToList()
            });
    }

    public static IEnumerable<QuestionCacheItemInPage> QuestionsInCategories(
        this IEnumerable<QuestionCacheItem> questions)
    {
        var questionsArray = questions as QuestionCacheItem[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionCacheItemInPage
            {
                PageCacheItem = c,
                QuestionCacheItems = questionsArray.Where(q => q.Pages.Any(x => x.Id == c.Id))
                    .ToList()
            });
    }
}

[DebuggerDisplay("{PageCacheItem.Name} {Questions.Count}")]
public class QuestionsInPage
{
    public PageCacheItem PageCacheItem;
    public IList<Question> Questions;
}

public class QuestionCacheItemInPage
{
    public PageCacheItem PageCacheItem;
    public IList<QuestionCacheItem> QuestionCacheItems;
}
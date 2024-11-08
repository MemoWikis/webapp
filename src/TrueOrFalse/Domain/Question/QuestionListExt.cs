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
                EntityCache.GetCategories(
                    q.Categories.Where(c => c != null)
                        .Select(c => c.Id)))
            .Distinct();

    public static IEnumerable<PageCacheItem> GetAllCategories(
        this IEnumerable<QuestionCacheItem> questions) =>
        questions.SelectMany(q =>
                EntityCache.GetCategories(
                    q.Pages.Where(c => c != null)
                        .Select(c => c.Id)))
            .Distinct();

    public static IEnumerable<QuestionsInCategory> QuestionsInCategories(
        this IEnumerable<Question> questions)
    {
        var questionsArray = questions as Question[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionsInCategory
            {
                PageCacheItem = c,
                Questions = questionsArray.Where(q => q.Categories.Any(x => x.Id == c.Id)).ToList()
            });
    }

    public static IEnumerable<QuestionCacheItemInCategory> QuestionsInCategories(
        this IEnumerable<QuestionCacheItem> questions)
    {
        var questionsArray = questions as QuestionCacheItem[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionCacheItemInCategory
            {
                PageCacheItem = c,
                QuestionCacheItems = questionsArray.Where(q => q.Pages.Any(x => x.Id == c.Id))
                    .ToList()
            });
    }

    public static IList<Question> AllowedForUser(this IEnumerable<Question> questions, User user) =>
        questions.Where(q => q.Visibility == QuestionVisibility.All || q.Creator == user).ToList();

    public static IList<Question> PrivateForUserOnly(
        this IEnumerable<Question> questions,
        User user) =>
        questions.Where(q => q.Visibility == QuestionVisibility.Owner && q.Creator == user)
            .ToList();
}

[DebuggerDisplay("{PageCacheItem.Name} {Questions.Count}")]
public class QuestionsInCategory
{
    public PageCacheItem PageCacheItem;
    public IList<Question> Questions;
}

public class QuestionCacheItemInCategory
{
    public PageCacheItem PageCacheItem;
    public IList<QuestionCacheItem> QuestionCacheItems;
}
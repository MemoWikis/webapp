using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class QuestionListExt
{
    public static Question ById(this IEnumerable<Question> questions, int id) => 
        questions.FirstOrDefault(question => question.Id == id);

    public static IList<int> GetIds(this IEnumerable<Question> questions) => 
        questions.Select(q => q.Id).ToList();

    public static IEnumerable<CategoryCacheItem> GetAllCategories(this IEnumerable<Question> questions) => 
        questions.SelectMany(q => 
            EntityCache.GetCategoryCacheItems(
                q.Categories.Where(c => c != null)
                    .Select(c => c.Id)))
            .Distinct();

    public static IEnumerable<QuestionsInCategory> QuestionsInCategories(this IEnumerable<Question> questions)
    {
        var questionsArray = questions as Question[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionsInCategory{
                CategoryCacheItem = c,
                Questions = questionsArray.Where(q => q.Categories.Any(x => x.Id == c.Id)).ToList()
            });
    }

    public static IList<Question> AllowedForUser(this IEnumerable<Question> questions, User user) =>
        questions.Where(q => q.Visibility == QuestionVisibility.All || q.Creator == user).ToList();

    public static IList<Question> PrivateForUserOnly(this IEnumerable<Question> questions, User user) =>
        questions.Where(q => q.Visibility == QuestionVisibility.Owner && q.Creator == user).ToList();
}

[DebuggerDisplay("{CategoryCacheItem.Name} {Questions.Count}")]
public class QuestionsInCategory
{
    public CategoryCacheItem CategoryCacheItem;
    public IList<Question> Questions;
}
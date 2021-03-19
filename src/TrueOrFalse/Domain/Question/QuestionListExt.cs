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
        questions.SelectMany(q => EntityCache.GetCategoryCacheItems(q.CategoriesIds)).Where(c => c != null).Distinct();

    public static IEnumerable<QuestionsInCategory> QuestionsInCategories(this IEnumerable<Question> questions)
    {
        var questionsArray = questions as Question[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionsInCategory{
                Category = c,
                Questions = questionsArray.Where(q => q.CategoriesIds.Any(x => x == c)).ToList()
            });
    }

    public static IList<Question> AllowedForUser(this IEnumerable<Question> questions, User user) =>
        questions.Where(q => q.Visibility == QuestionVisibility.All || q.Creator == user).ToList();

    public static IList<Question> PrivateForUserOnly(this IEnumerable<Question> questions, User user) =>
        questions.Where(q => q.Visibility == QuestionVisibility.Owner && q.Creator == user).ToList();
}

[DebuggerDisplay("{Category.Name} {Questions.Count}")]
public class QuestionsInCategory
{
    public CategoryCacheItem Category;
    public IList<Question> Questions;
}
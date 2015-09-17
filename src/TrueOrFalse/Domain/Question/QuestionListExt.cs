using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class QuestionListExt
{
    public static Question ById(this IEnumerable<Question> questions, int id)
    {
        return questions.FirstOrDefault(question => question.Id == id);
    }
        
    public static IList<int> GetIds(this IEnumerable<Question> questions)
    {
        return questions.Select(q => q.Id).ToList();
    }

    public static IEnumerable<Category> GetAllCategories(this IEnumerable<Question> questions)
    {
        return questions.SelectMany(q => q.Categories).Where(c => c != null).Distinct();
    }

    public static IEnumerable<QuestionsInCategory> QuestionsInCategories(this IEnumerable<Question> questions)
    {
        var questionsArray = questions as Question[] ?? questions.ToArray();

        return questionsArray.GetAllCategories()
            .Select(c => new QuestionsInCategory{
                Category = c,
                Questions = questionsArray.Where(q => q.Categories.Any(x => x == c)).ToList()
            });
    }
}

[DebuggerDisplay("{Category.Name} {Questions.Count}")]
public class QuestionsInCategory
{
    public Category Category;
    public IList<Question> Questions;
}
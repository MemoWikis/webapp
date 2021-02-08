using System.Linq;

public class GetPrimaryCategory
{
    public static Category GetForQuestion(Question question)
    {
        if (question.Categories.Any())
            return question.Categories.First();

        return null;
    }
}

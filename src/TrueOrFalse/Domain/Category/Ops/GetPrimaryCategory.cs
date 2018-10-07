using System.Linq;

public class GetPrimaryCategory
{
    public static Category GetForQuestion(Question question)
    {
        if (question.SetsTop5.Any())
        {
            foreach (var set in question.SetsTop5)
            {
                if (set.Categories.Any())
                    return set.Categories.First();
            }
        }
        if (question.Categories.Any())
            return question.Categories.First();

        return null;
    }
}

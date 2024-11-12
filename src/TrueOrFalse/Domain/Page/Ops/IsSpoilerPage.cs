
public class IsSpoilerPage
{
    public static bool Yes(string categoryText, QuestionCacheItem question)
    {
        var solution = GetQuestionSolution.Run(question);
        var correctAnswer = solution.CorrectAnswer();

        return NormalizeString(categoryText) == NormalizeString(correctAnswer);
    }

    private static string NormalizeString(string input)
    {
        if (input == null)
            return null;

        return input.ToLower().Replace(" ", "");
    }
}

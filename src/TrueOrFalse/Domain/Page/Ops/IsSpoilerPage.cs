
public class IsSpoilerPage
{
    public static bool Yes(string pageText, QuestionCacheItem question)
    {
        var solution = GetQuestionSolution.Run(question);
        var correctAnswer = solution.CorrectAnswer();

        return NormalizeString(pageText) == NormalizeString(correctAnswer);
    }

    private static string NormalizeString(string input)
    {
        if (input == null)
            return null;

        return input.ToLower().Replace(" ", "");
    }
}


public class IsSpoilerCategory
{
    public static bool Yes(string categoryText, Question question)
    {
        var solution = new GetQuestionSolution().Run(question);
        var correctAnswer = solution.CorrectAnswer();

        return NormalizeString(categoryText) == NormalizeString(correctAnswer);
    }

    private static string NormalizeString(string input)
    {
        return input.ToLower().Replace(" ","");
    }
}

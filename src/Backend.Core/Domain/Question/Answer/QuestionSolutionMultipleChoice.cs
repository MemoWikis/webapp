public class QuestionSolutionMultipleChoice : QuestionSolution
{
    private const string AnswerListDelimiter = "</br>";
    public List<Choice> Choices = new List<Choice>();

    public override bool IsCorrect(string answer)
    {
        var answers = answer
            .Split(new string[] { "%seperate&xyz%" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim());
        var solutions = CorrectAnswer()
            .Split(new[] { AnswerListDelimiter }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim());
        return answers.OrderBy(t => t).SequenceEqual(solutions.OrderBy(t => t));
    }

    public override string CorrectAnswer()
    {
        string correctAnswer = AnswerListDelimiter;
        foreach (var singleChoice in Choices)
        {
            if (singleChoice.IsCorrect)
            {
                correctAnswer += singleChoice.Text;
                if (singleChoice != Choices[Choices.Count - 1])
                    correctAnswer += AnswerListDelimiter;
            }
        }

        return correctAnswer;
    }

    public override string GetCorrectAnswerAsHtml()
    {
        string htmlListItems;

        var correctAnswer = CorrectAnswer();

        if (correctAnswer == AnswerListDelimiter)
            return "";

        if (!correctAnswer.Contains(AnswerListDelimiter))
        {
            htmlListItems = $"<li>{correctAnswer}</li>";
        }
        else
        {
            htmlListItems = correctAnswer
                .Split(new[] { AnswerListDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => $"<li>{a}</li>")
                .Aggregate((a, b) => a + b);
        }

        return $"<ul>{htmlListItems}</ul>";
    }
}
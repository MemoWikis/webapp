using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class QuestionSolutionMultipleChoice_SingleSolution : QuestionSolution
{
    public List<string> Choices;

    public override bool IsCorrect(string answer)
    {
        return Choices.First().Trim() == answer.Trim();
    }

    public override string CorrectAnswer()
    {
        if (!Choices.Any())
            return "";

        return Choices.First();
    }
}
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Script.Serialization;

public class QuestionSolutionSequence : QuestionSolution
{
    public Dictionary<string, string> Rows;

    public override bool IsCorrect(string answer)
    {
        var values = new JavaScriptSerializer().Deserialize<string[]>(answer.Trim());
        return values.SequenceEqual(Rows.Values);
    }

    public override string CorrectAnswer()
    {
        return string.Join(", ", Rows.Values);
    }
}
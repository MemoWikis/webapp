using System.Collections.Specialized;
using System.ComponentModel;
using TrueOrFalse;

public class QuestionSolutionExact : QuestionSolution
{
    [DisplayName("Antwort")]
    public string Text { get; set; }

    public SolutionMetadata Metadata = new SolutionMetadata();

    public override bool IsCorrect(string answer)
    {
        return Text == answer;
    }

    public override string CorrectAnswer()
    {
        return Text;
    }

    public void FillFromPostData(NameValueCollection postData)
    {
        Text = postData["Text"];
    }
}
using System.Collections.Specialized;
using System.ComponentModel;

public class QuestionSoulutionExact : QuestionSolution
{
    [DisplayName("Antwort")]
    public string Text { get; set; }

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
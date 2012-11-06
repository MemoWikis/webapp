using System.Collections.Specialized;

public class QuestionSoulutionExact : QuestionSolution
{
    public string Text;

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
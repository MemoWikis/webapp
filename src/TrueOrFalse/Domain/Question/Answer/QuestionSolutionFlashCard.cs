using System.Collections.Specialized;
using TrueOrFalse.Web;

public class QuestionSolutionFlashCard : QuestionSolution
{
    public string Text;

    public void FillFromPostData(NameValueCollection postData)
    {
        Text = postData.Get("FlashCardContent");
    }

    public override bool IsCorrect(string answer)
    {   
        return answer == "(Antwort gewusst)";
    }

    public override string CorrectAnswer()
    {
        return Text;
    }

    public override string GetCorrectAnswerAsHtml()
    {
        return MarkdownMarkdig.ToHtml(Text);
    }
}
using System.Collections.Specialized;

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
}
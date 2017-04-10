using System.Collections.Specialized;

public class QuestionSolutionFlashCard : QuestionSolution
{
    public string FlashCardContent;

    public void FillFromPostData(NameValueCollection postData)
    {
        FlashCardContent = postData.Get("FlashCardContent");
    }

    public override bool IsCorrect(string answer)
    {
        return false;
    }

    public override string CorrectAnswer()
    {
        return FlashCardContent;
    }
}
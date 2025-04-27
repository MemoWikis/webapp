public class QuestionSolutionFlashCard : QuestionSolution
{
    public string Text;

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
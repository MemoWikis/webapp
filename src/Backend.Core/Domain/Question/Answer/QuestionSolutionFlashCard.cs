public class QuestionSolutionFlashCard : QuestionSolution
{
    public string Text;

    public override bool IsCorrect(string answer)
    {
        if (Enum.TryParse<FlashCardAnswerType>(answer, out var answerType))
        {
            return answerType == FlashCardAnswerType.Known;
        }
        return false;
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
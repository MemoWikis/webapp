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
}
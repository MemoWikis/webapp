public abstract class QuestionSolution
{
    public abstract bool IsCorrect(string answer);

    public abstract string CorrectAnswer();

    public virtual string GetCorrectAnswerAsHtml() => CorrectAnswer();
    public virtual string GetAnswerForSEO() => CorrectAnswer();
}

public class ProbabilityCalc_Category
{
    public static int Run(IList<Answer> answers)
    {
        if (!answers.Any())
            return 50;

        decimal answeredCorrectly =
            answers.Count(x => x.AnswerredCorrectly != AnswerCorrectness.False);

        return (int)((answeredCorrectly / answers.Count()) * 100);
    }
}
public class ProbabilityCalc_Question : IRegisterAsInstancePerLifetime
{
    public static int Run(IList<Answer> answers, bool useFirstAnswerPerUserOnly = false)
    {
        if (!answers.Any())
            return 30;

        if (useFirstAnswerPerUserOnly)
            answers = answers.GroupBy(a => a.UserId)
                .Select(g => g.OrderBy(a => a.DateCreated).First()).ToList();

        decimal answeredCorrectly =
            answers.Count(x => x.AnswerredCorrectly != AnswerCorrectness.False);

        return (int)((answeredCorrectly / answers.Count()) * 100);
    }
}
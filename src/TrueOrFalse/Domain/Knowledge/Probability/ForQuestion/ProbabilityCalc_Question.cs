using System.Collections.Generic;
using System.Linq;

public class ProbabilityCalc_Question : IRegisterAsInstancePerLifetime
{
    public static int Run(IList<Answer> answers)
    {
        if (!answers.Any())
            return 30;

        decimal answeredCorrectly = 
            answers.Count(x => x.AnswerredCorrectly != AnswerCorrectness.False);

        return (int)((answeredCorrectly / answers.Count()) * 100);
    }
}
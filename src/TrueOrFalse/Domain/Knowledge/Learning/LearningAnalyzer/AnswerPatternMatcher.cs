using System.Collections.Generic;

public class AnswerPatternMatcher
{
    public static List<AnswerPatternMatch> Run(List<Answer> listOfAnswers)
    {
        var result = new List<AnswerPatternMatch>();

        AnswerPatternRepo.GetAll().ForEach(p =>
        {
            if (p.IsMatch(listOfAnswers))
                result.Add(new AnswerPatternMatch
                {
                    Name = p.Name,
                    Type = p.GetType(),
                    Pattern = p
                });
        });

        return result;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

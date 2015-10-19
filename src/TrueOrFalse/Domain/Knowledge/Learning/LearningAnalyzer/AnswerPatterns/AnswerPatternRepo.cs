using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnswerPatternRepo
{
    public static List<IAnswerPattern> GetAll()
    {
        return new List<IAnswerPattern>()
        {
            new XDaysExactly2(),
            new XDaysExactly3(),
            new XDaysExactly4(),
            new XDaysExactly5(),
            new XDaysExactly6OrMore(),

            new MaxStreakOf2(),
            new MaxStreakOf3(),
            new MaxStreakOf4(),
            new MaxStreakOf5(),
            new MaxStreakOfMoreThan5()
        };
    }
}

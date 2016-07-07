using System;
using System.Collections.Generic;
using System.Linq;
using static System.Boolean;

[Serializable]
public class LeitnerQuestion
{
    public IList<LeitnerAnswer> History;

    public int Complexity;
    public LeitnerBox Box;

    public int NextRepetitionDayNumber = 0;

    public static IEnumerable<LeitnerQuestion> CreateQuestions(int amount = 100)
    {
        return Enumerable
            .Range(start: 0, count: 100)
            .Select(i =>
            {
                var complexity = 50;
                if (i%4 == 1)
                    complexity = 25;

                if (i%4 == 2)
                    complexity = 75;

                return new LeitnerQuestion
                {
                    Complexity = complexity
                };

            });
    }

    public bool Answer()
    {
        return Convert.ToBoolean(new Random((int) DateTime.Now.Ticks).Next(0,2));
    }
}
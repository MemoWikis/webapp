using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LeitnerQuestion
{
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
}
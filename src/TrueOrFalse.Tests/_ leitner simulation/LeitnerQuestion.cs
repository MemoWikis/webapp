using System;
using System.Collections.Generic;
using System.Linq;
using static System.Boolean;

[Serializable]
public class LeitnerQuestion
{
    public IList<LeitnerAnswer> History = new List<LeitnerAnswer>();

    public int Complexity;
    public LeitnerBox Box;

    public int Probability;

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
                    Complexity = complexity,
                    Probability = 100 - complexity
                };

            });
    }

    public bool Answer(int dayNumber)
    {
        var r = new Random();
        
        var wasCorrect = r.Next(100) < Probability;

        History.Add(new LeitnerAnswer {Day = dayNumber, WasCorrect = wasCorrect});

        return wasCorrect;
    }

    public void UpdateProbability(int currentDay)
    {
        var offset = TimeSpan.FromDays(currentDay - (History.Any() ? History.Last().Day : 1)).TotalMinutes;

        var stability = 2048;

        Probability = ProbabilityCalc_Curve.GetProbability(offset, stability, Probability);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

[Serializable]
public class LeitnerQuestion
{
    public IList<LeitnerAnswer> History = new List<LeitnerAnswer>();

    public int Complexity;
    public LeitnerBox Box;

    public int NextRepetitionDayNumber = 0;

    public static IEnumerable<LeitnerQuestion> CreateQuestions(int amount = 100)
    {
        return Enumerable
            .Range(start: 0, count: amount)
            .Select(i =>
            {
                var complexity = 50;
                if (i%4 == 1)
                    complexity = 25;

                if (i%4 == 2)
                    complexity = 75;

                return new LeitnerQuestion{ Complexity = complexity };
            });
    }

    public bool Answer(int dayNumber)
    {

        var probability = GetProbability(dayNumber, History);
        var rd = LeitnerSimulation.Random.Next(0, 100);
        var wasCorrect =  rd < probability;
        Console.WriteLine(rd);
        Logg.r().Information($"Day: {dayNumber} Rnd:{rd} Prob:{probability}");

        History.Add(new LeitnerAnswer
        {
            Day = dayNumber,
            WasCorrect = wasCorrect,
            ProbabilityBefore = probability
        });

        return wasCorrect;
    }

    public int GetProbability(int currentDay, IList<LeitnerAnswer> history)
    {
        var offsetInMinutes = TimeSpan.FromDays(currentDay - (History.Any() ? History.Last().Day : 1)).TotalMinutes;

        history.ForEach(a => a.SetResultFor_GetAnswerOffsetInMinutes(offsetInMinutes));

        var stability = ProbabilityCalc_Curve_HalfLife_24h.GetStabilityModificator(history.ToList<IAnswered>());

        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability, 100);
    }
}
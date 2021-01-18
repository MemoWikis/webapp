using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Util;
using ObjectDumper;

[Serializable]
public class LeitnerQuestion
{
    public IList<LeitnerAnswer> History = new List<LeitnerAnswer>();

    public int Complexity;
    public LeitnerBox Box;

    public static IList<LeitnerQuestion> CreateQuestions(int amount)
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
            })
            .ToList();
    }

    private static readonly Random _random = new Random();
    
    public bool Answer(int dayNumber)
    {
        var probability = GetProbability(dayNumber, History);
        var random = _random.Next(0, 100);
        var wasCorrect =  random < probability;

        History.Add(new LeitnerAnswer
        {
            DayAnswered = dayNumber,
            WasCorrect = wasCorrect,
            ProbabilityBefore = probability
        });

        return wasCorrect;
    }

    public int GetProbability(int currentDay, IList<LeitnerAnswer> history)
    {
        history.ForEach(a => a.SetResultFor_GetAnswerOffsetInMinutes(currentDay));

        var offsetInMinutes = 0;
        if (history.Any())
            offsetInMinutes = (int) history
                    .OrderByDescending(h => h.DayAnswered)
                    .Last()
                    .GetAnswerOffsetInMinutes();

        var stability = 
            ProbabilityCalc_Curve_HalfLife_24h.Stability +
            ProbabilityCalc_Curve_HalfLife_24h.GetStabilityModificator(history.ToList<IAnswered>());

        var initialProbabilityValue = 100;
        if (!history.Any())
            initialProbabilityValue = LeitnerSimulation.InitialProbability;

        if(currentDay == 10)
            Debugger.Break();

        var probability = ProbabilityCalc_Curve.GetProbability(
            offsetInMinutes, 
            stability, 
            initialProbabilityValue
        );

        return probability;
    }
}
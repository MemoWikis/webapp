﻿using System;
using System.Linq;
using Common.Logging.Configuration;
using NUnit.Framework;

[TestFixture]
public class LeitnerSimulationTest
{
    [Test]
    public void Start()
    {
        var leitnerSimulation = new LeitnerSimulation();
        int numberOfDays = 20;
        leitnerSimulation.Start(numberOfDays: numberOfDays);

        Assert.That(leitnerSimulation.Days.Count, Is.EqualTo(numberOfDays));

        foreach (var day in leitnerSimulation.Days)
        {
            Console.WriteLine($"== Day == {day.Number}");
            foreach (var box in day.BoxesBefore)
            {
                Console.WriteLine($"Box{(box.ToRepeat ? "*" : "")}{box.Number}: {box.Questions.Count} / {day.BoxesAfter.ByNumber(box.Number).Questions.Count}");
            }
        }
    }
}

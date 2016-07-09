using System;
using System.Linq;
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

        foreach (var day in leitnerSimulation.Days.Skip(1))
        {
            Console.WriteLine($"== Day == {day.Number}");
            foreach (var box in day.Boxes)
            {
                Console.WriteLine($"Box: {box.Number} {box.Questions.Count}");
            }
        }
    }
}

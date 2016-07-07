using NUnit.Framework;

[TestFixture]
public class LeitnerSimulationTest
{
    [Test]
    public void Start()
    {
        var leitnerSimulation = new LeitnerSimulation();
        leitnerSimulation.Start(numberOfDays:10);

        Assert.That(leitnerSimulation.Days.Count, Is.EqualTo(10));

        //start simulation

        //print result 
    }
}

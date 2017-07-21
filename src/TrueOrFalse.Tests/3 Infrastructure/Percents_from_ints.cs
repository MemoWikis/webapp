using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Percents_from_ints
{
    [Test]
    public void Test_distribution()
    {
        //var test = Percents.FromInts(new[] {22d, 77d});
        Assert.That(Percents.FromInts(new[] {0.225d, 0.775d}).Sum(), Is.EqualTo(100) );

        Assert.That(Percents.FromInts(new[] { 99, 3}).Sum(), Is.EqualTo(100));
        Assert.That(Percents.FromInts(new[] { 101, 3 }).Sum(), Is.EqualTo(100));
        Assert.That(Percents.FromInts(new[] { 102, 3 }).Sum(), Is.EqualTo(100));
        Assert.That(Percents.FromInts(new[] { 775, 225 }).Sum(), Is.EqualTo(100));

        Percents.FromIntsd(new List<PercentAction>
        {
            new PercentAction{Value = 775, Action = p => Console.WriteLine(p)},
            new PercentAction{Value = 225, Action = p => Console.WriteLine(p)},
        });
    }
}

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
        Assert.That(PercentageShares.FromAbsoluteShares(new[] { 99, 3}).Sum(), Is.EqualTo(100));
        Assert.That(PercentageShares.FromAbsoluteShares(new[] { 101, 3 }).Sum(), Is.EqualTo(100));
        Assert.That(PercentageShares.FromAbsoluteShares(new[] { 102, 3 }).Sum(), Is.EqualTo(100));
        Assert.That(PercentageShares.FromAbsoluteShares(new[] { 775, 225 }).Sum(), Is.EqualTo(100));

        PercentageShares.FromAbsoluteShares(new List<ValueWithResultAction>
        {
            new ValueWithResultAction{AbsoluteValue = 775, ActionForPercentage = p => Console.WriteLine(p)},
            new ValueWithResultAction{AbsoluteValue = 225, ActionForPercentage = p => Console.WriteLine(p)},
        });
    }
}

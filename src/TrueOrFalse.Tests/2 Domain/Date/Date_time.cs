using System;
using NUnit.Framework;

public class Date_time
{
    [Test]
    public void Should_compare_and_parse_time()
    {
        Assert.That(new Time(7, 3).ToString(), Is.EqualTo("07:03"));
        
        Assert.That(Time.Parse("12:41").ToString(), Is.EqualTo("12:41"));
        Assert.That(Time.Parse("2:41").ToString(), Is.EqualTo("02:41"));
        Assert.That(Time.Parse("2.41").ToString(), Is.EqualTo("02:41"));

        Assert.That(Time.Parse("Uhr").ToString(), Is.EqualTo("00:00"));

        Assert.That(Time.Parse("11:08").SetTime(new DateTime(2012, 12, 7, 14, 23, 0)), 
            Is.EqualTo(new DateTime(2012, 12, 7, 11, 08, 0)));
    }

    [Test]
    public void Should_compare_times()
    {
        Assert.That(Time.New(12, 41), Is.LessThan(Time.New(12, 42)));
        Assert.That(Time.New(12, 41), Is.GreaterThan(Time.New(12, 39)));
        Assert.That(Time.New(11, 41), Is.EqualTo(Time.New(11, 41)));

        Assert.That(Time.New(11, 00), Is.GreaterThan(Time.New(2, 00)));

        Assert.That(Time.New(11, 00) > Time.New(2, 00), Is.True);
        Assert.That(Time.New(11, 00) < Time.New(12, 00), Is.True);

        Assert.That(Time.New(11, 00) <= Time.New(12, 00), Is.True);
        Assert.That(Time.New(11, 00) <= Time.New(11, 00), Is.True);

        Assert.That(Time.New(11, 00) >= Time.New(9, 00), Is.True);
        Assert.That(Time.New(11, 00) >= Time.New(11, 00), Is.True);
    }

    public void Should_detect_if_is_in_period()
    {
        var periodA = new Period(new Time(22,10), new Time(22,30));

        Assert.That(periodA.IsInPeriod(new Time(10, 00)), Is.False);
    }
}
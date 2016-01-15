﻿using System;
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

    [Test]
    public void Should_detect_if_is_in_period()
    {
        var periodA = new Period(new Time(22,10), new Time(22,30));

        Assert.That(periodA.IsInPeriod(new Time(10, 00)), Is.False);
        Assert.That(periodA.IsInPeriod(new Time(22, 20)), Is.True);
        Assert.That(periodA.IsInPeriod(new Time(22, 10)), Is.True);

        var periodB = new Period(new Time(22, 00), new Time(6, 00));
        Assert.That(periodB.IsInPeriod(new Time(21, 00)), Is.False);
        Assert.That(periodB.IsInPeriod(new Time(7, 00)), Is.False);
        Assert.That(periodB.IsInPeriod(new Time(22, 00)), Is.True);
        Assert.That(periodB.IsInPeriod(new Time(23, 00)), Is.True);
        Assert.That(periodB.IsInPeriod(new Time(5, 00)), Is.True);
    }

    [Test]
    public void Should_move_time_foraward_in_dateTimeX()
    {
        Assert.That(DateTimeX.Now().Second, Is.EqualTo(DateTime.Now.Second));
        DateTimeX.Forward(mins:30);
        Assert.That((DateTimeX.Now() - DateTime.Now).TotalSeconds, Is.EqualTo(30 * 60));
        DateTimeX.Forward(hours:5);
        Assert.That((DateTimeX.Now() - DateTime.Now).TotalSeconds, Is.EqualTo(30 * 60 + 5 * 3600));
        DateTimeX.ResetOffset();
        Assert.That(DateTimeX.Now().Second, Is.EqualTo(DateTime.Now.Second));
    }
}
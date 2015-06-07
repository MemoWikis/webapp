using System;
using NUnit.Framework;

public class Date_time
{
    [Test]
    public void Various()
    {
        Assert.That(new Time(7, 3).ToString(), Is.EqualTo("07:03"));
        
        Assert.That(Time.Parse("12:41").ToString(), Is.EqualTo("12:41"));
        Assert.That(Time.Parse("2:41").ToString(), Is.EqualTo("02:41"));
        Assert.That(Time.Parse("2.41").ToString(), Is.EqualTo("02:41"));

        Assert.That(Time.Parse("Uhr").ToString(), Is.EqualTo("00:00"));

        Assert.That(Time.Parse("11:08").SetTime(new DateTime(2012, 12, 7, 14, 23, 0)), 
            Is.EqualTo(new DateTime(2012, 12, 7, 11, 08, 0)));
    }
}
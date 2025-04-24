using System;
using NUnit.Framework;

namespace TrueOrFalse.Utilities.Tests;

public class TimeElapsedAsTextTests : BaseTest
{
    [Test]
    public void Should_format_correctly()
    {
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMilliseconds(-1)), Is.EqualTo("weniger als einer Minute"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-1)), Is.EqualTo("einer Minute"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-1.5)), Is.EqualTo("2 Minuten"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-5)), Is.EqualTo("5 Minuten"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-20.4)), Is.EqualTo("20 Minuten"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-20.5)), Is.EqualTo("21 Minuten"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-1).AddMinutes(-10)), Is.EqualTo("einer Stunde"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-1).AddMinutes(-30)), Is.EqualTo("2 Stunden"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-2).AddMinutes(-20)), Is.EqualTo("2 Stunden"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-4).AddMinutes(-20)), Is.EqualTo("4 Stunden"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-4).AddMinutes(-20)), Is.EqualTo("4 Stunden"));
        Assert.That(TimeElapsedAsText.Run(new DateTime(2014, 5, 16, 11, 00, 00), new DateTime(2014, 5, 17, 10, 00, 00)), Is.EqualTo("23 Stunden"));
        Assert.That(TimeElapsedAsText.Run(new DateTime(2014, 5, 16, 10, 00, 00), new DateTime(2014, 5, 17, 11, 00, 00)), Is.EqualTo("einem Tag"));
        Assert.That(TimeElapsedAsText.Run(new DateTime(2014, 5, 16, 23, 59, 00), new DateTime(2014, 5, 18, 00, 00, 01)), Is.EqualTo("2 Tagen"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-31)), Is.EqualTo("einem Monat"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-87)), Is.EqualTo("3 Monaten"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-96)), Is.EqualTo("3 Monaten"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-400)), Is.EqualTo("einem Jahr"));
        Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-700)), Is.EqualTo("2 Jahren"));
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Tests;

namespace TrueOrFalse.Utilities.Tests
{
    public class TimeElapsedAsTextTests : BaseTest
    {
        [Test]
        public void Should_format_correctly()
        {
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMilliseconds(-1)), Is.EqualTo("vor Augenblicken"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddSeconds(-5)), Is.EqualTo("5 Sekunden"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-5)), Is.EqualTo("5 Minuten"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddMinutes(-20)), Is.EqualTo("20 Minuten"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-1).AddMinutes(-10)), Is.EqualTo("einer Stunde"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-2).AddMinutes(-20)), Is.EqualTo("2 Stunden"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-4).AddMinutes(-20)), Is.EqualTo("4 Stunden"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-31)), Is.EqualTo("einem Monat"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-87)), Is.EqualTo("3 Monate"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddDays(-96)), Is.EqualTo("3 Monate"));            
        }
    }
}

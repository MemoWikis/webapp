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
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-1).AddMinutes(-10)), Is.EqualTo("1 Stunde"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-2).AddMinutes(-20)), Is.EqualTo("2 Stunden"));
            Assert.That(TimeElapsedAsText.Run(DateTime.Now.AddHours(-2).AddMinutes(-20)), Is.EqualTo("4 Stunden"));
        }
    }
}

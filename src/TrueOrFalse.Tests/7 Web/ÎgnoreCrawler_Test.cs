using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Tools;

namespace TrueOrFalse.Tests._7_Web
{
    class ÎgnoreCrawler_Test
    {
        [Test]
        public void Should_load_all_cralwers()
        {
            var allCrawlers = IgnoreLog.GetCrawlers();

            Assert.That(allCrawlers.Count, Is.GreaterThanOrEqualTo(1));
        }
    }
}

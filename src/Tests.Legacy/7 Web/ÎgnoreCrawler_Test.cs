using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tools;

class ÎgnoreCrawler_Test
{
    [Test]
    public void Should_load_all_cralwers()
    {
        var allCrawlers = IgnoreLog.GetCrawlers();

        Assert.That(allCrawlers.Count, Is.GreaterThanOrEqualTo(1));
    }
}
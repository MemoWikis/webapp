using NUnit.Framework;

public class Crawler_tests
{
    [Test]
    public void Should_load_all_cralwers()
    {
        var allCrawlers = CrawlerRepo.GetAll();

        Assert.That(allCrawlers.Count, Is.GreaterThanOrEqualTo(199));
    }
}

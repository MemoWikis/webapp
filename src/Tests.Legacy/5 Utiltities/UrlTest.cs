using NUnit.Framework;
using TrueOrFalse.Frontend.Web.Code;

namespace TrueOrFalse.Tests;

public class UrlTest : BaseTest
{
    [Test]
    public void Should_detect_wikipedia_url()
    {
        Assert.That(Links.IsLinkToWikipedia("http://wikipedia.de"), Is.True);
        Assert.That(Links.IsLinkToWikipedia("https://de.wikipedia.com"), Is.True);
        Assert.That(Links.IsLinkToWikipedia("https://en.wikipedia.org"), Is.True);


        //false
        Assert.That(Links.IsLinkToWikipedia(null), Is.False);
        Assert.That(Links.IsLinkToWikipedia(""), Is.False);
        Assert.That(Links.IsLinkToWikipedia("https://berlin.wikipedia"), Is.False);
        Assert.That(Links.IsLinkToWikipedia("https://wikia.de"), Is.False);
    }
}
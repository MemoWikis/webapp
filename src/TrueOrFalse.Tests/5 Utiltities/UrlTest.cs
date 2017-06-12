using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Frontend.Web.Code;

namespace TrueOrFalse.Tests
{
    public class UrlTest : BaseTest
    {
        [Test]
        public void Should_transform_input_to_valid_url_with_protocol()
        {
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol("http://memucho.de"), Is.EqualTo("http://memucho.de"));
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol("hTtP://memucho.de"), Is.EqualTo("http://memucho.de"));
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol("https://memucho.de"), Is.EqualTo("https://memucho.de"));
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol("HTTPS://memucho.de"), Is.EqualTo("https://memucho.de"));
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol("memucho.de"), Is.EqualTo("http://memucho.de"));
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol("x.de"), Is.EqualTo("http://x.de"));
            Assert.That(EditCategoryModel.T_ToUrlWithProtocol(""), Is.EqualTo(""));
        }

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
}


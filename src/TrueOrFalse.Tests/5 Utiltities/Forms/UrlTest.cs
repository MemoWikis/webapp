using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarkdownSharp;
using NUnit.Framework;
using TrueOrFalse.Web;

namespace TrueOrFalse.Tests
{
    /// <summary>Just for playing arouund and as reference..</summary>
    public class MarkdownSharpTests
    {
        [Test]
        public void Should_transform_markdown_links_to_html()
        {
            const string input = "Have you visited <http://www.example.com> before?";
            const string expected =
                "<p>Have you visited <a href=\"http://www.example.com\">http://www.example.com</a> before?</p>\n";

            string actual = new Markdown().Transform(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_auto_transform_markdown_links_to_html()
        {
            const string input = "Have you visited http://www.example.com before?";
            const string expected =
                "<p>Have you visited <a href=\"http://www.example.com\">http://www.example.com</a> before?</p>\n";

            string actual = new Markdown(new MarkdownOptions()) {AutoHyperlink = true}.Transform(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_transform_img_to_html()
        {
            var input = "![enter image description here][1]\r\n\r\n\r\n[1]: /Images/Questions/test.jpg";
            var expected = "<p><img src=\"/Images/Questions/test.jpg\" alt=\"enter image description here\"></p>\n";

            var markdownResult = new Markdown(new MarkdownOptions()) { AutoHyperlink = true }.Transform(input);

            var brokenImgTag = "<p><img src=\"(.*)\"</p>";
            var regexMatch = Regex.Match(markdownResult, brokenImgTag);

            var repairedString = regexMatch.ToString().Replace("\"</p>", "\"></p>");
            var actual = markdownResult.Replace(regexMatch.ToString(), repairedString);

            Assert.AreEqual(expected, actual);
        }
    }
}

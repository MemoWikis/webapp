using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkdownSharp;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    /// <summary>Just for playing arouund and as reference..</summary>
    public class MarkdownTests
    {
        [Test]
        public void Should_transform_markdown_links_to_html()
        {
            const string input = "Have you visited <http://www.example.com> before?";
            const string expected = "<p>Have you visited <a href=\"http://www.example.com\">http://www.example.com</a> before?</p>\n";

            string actual = new Markdown().Transform(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_auto_transform_markdown_links_to_html()
        {
            const string input = "Have you visited http://www.example.com before?";
            const string expected = "<p>Have you visited <a href=\"http://www.example.com\">http://www.example.com</a> before?</p>\n";

            string actual = new Markdown(new MarkdownOptions()){AutoHyperlink = true}.Transform(input);
            Assert.AreEqual(expected, actual);
        }
    }
}

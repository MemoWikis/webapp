using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkdownSharp;
using Newtonsoft.Json;
using NUnit.Framework;
using TrueOrFalse.Web;

namespace TrueOrFalse.Tests
{
    public class MarkdigTests {

        [Ignore("")]
        [Test]
        public void Should_transform_markdown_to_html()
        {
            var result = MarkdownMarkdig.ToHtml("This is a text with some *emphasis*");
            Console.WriteLine(result);   // prints: <p>This is a text with some <em>emphasis</em></p>
        }

        [Ignore("")]
        [Test]
        public void Should_leave_partial_view_string_as_is()
        {
            var placeholder =
                ("{\n  \'array\': [\r\n    1,\r\n    2,\r\n    3\r\n  ],\r\n  \'boolean\': true,\r\n  \'null\': null,\r\n  \'number\': 123,\r\n  \'object\': {\r\n                \'a\': \'b\',\r\n    \'c\': \'d\',\r\n    \'e\': \'f\'\r\n  },\r\n  \'string\': \'Hello World\'\r\n}");
            Assert.That(MarkdownMarkdig.ToHtml(placeholder), Is.EqualTo("<p>" + placeholder + "</p>"));

        }

        [Ignore("Just for playing around")]
        [Test]
        public void Test_markdig()
        {
            Show_markdown_output(@"<div>input with div tag</div>");
            Show_markdown_output(@"<schnurps>custom tag</schnurps>
## jhkjhkhj");
            Show_markdown_output(@"plain text");
            Show_markdown_output(@"<div>div without closing tag");
            
            Show_markdown_output(@"## h2 heading");

            Show_markdown_output(@":::
### div from markdown
::: 
## jhkjhkhj");
            Show_markdown_output(@"[[sdfsdfsdf]] 
## jhkjhkhj");

        }

        public void Show_markdown_output(string markdown)
        {
            Console.WriteLine("Markdown:");
            Console.WriteLine(markdown);
            Console.WriteLine("Prints:");
            Console.WriteLine(MarkdownMarkdig.ToHtml(markdown));
            Console.WriteLine();
        }

        [Ignore("")]
        [Test]
        public static void ReplaceTemplate()
        {
            //var result = PartialParser.Run(@"
            //    Sample text for testing:
            //    abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ
            //    0123456789 _+-.,!@#$%^&*();\/|<>""'
            //    12345 -98.7 3.141 .6180 9,000 +42
            //    555.123.4567	+1-(800)-555-2468 
            //    [[[[foo@demo.net]]	bar.ba@test.co.uk]]
            //    [[www.demo.com	http://foo.co.uk/]]
            //    http://regexr.com/foo.html?q=bar
            //    [https://mediatemple.net]
            //");

        }

        [Ignore("")]
        [Test]
        public static void Serialize_template()
        {
            var templateJson = new TemplateJson {TemplateName = "Name"};
            var json = JsonConvert.SerializeObject(templateJson);
        }
    }
}

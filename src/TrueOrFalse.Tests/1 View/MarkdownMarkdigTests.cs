using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkdownSharp;
using NUnit.Framework;
using TrueOrFalse.Web;

namespace TrueOrFalse.Tests
{
    public class MarkdigTests {

        [Test]
        public void Shouldtransform_markdown_to_html()
        {
            var result = MarkdownMarkdig.ToHtml("This is a text with some *emphasis*");
            Console.WriteLine(result);   // prints: <p>This is a text with some <em>emphasis</em></p>
        }
    }
}

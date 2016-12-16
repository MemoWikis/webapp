using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;

namespace TrueOrFalse.Web
{
    public class MarkdownMarkdig
    {
        public static string ToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown);
        }
    }
}

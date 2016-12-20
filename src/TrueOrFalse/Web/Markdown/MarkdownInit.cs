using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkdownSharp;

namespace TrueOrFalse.Web
{
    public class MarkdownInit
    {
        public static MarkdownSharp.Markdown Run()
        {
            return new MarkdownSharp.Markdown(new MarkdownOptions { AutoHyperlink = true });
        }
    }
}

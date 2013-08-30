using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkdownSharp;

namespace TrueOrFalse.Web
{
    public class MardownInit
    {
        public static Markdown Run()
        {
            return new Markdown(new MarkdownOptions { AutoHyperlink = true });
        }
    }
}

using MarkdownSharp;

namespace TrueOrFalse.Web
{
    public class MarkdownInit
    {
        public static Markdown Run()
        {
            return new Markdown(new MarkdownOptions { AutoHyperlink = true });
        }
    }
}

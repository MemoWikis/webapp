using System.Security.AccessControl;
using TrueOrFalse.Web;

public class InlineTextModel : BaseContentModule
{
    public string Content;
    public string Raw;
    
    public InlineTextModel(string htmlContent)
    {
        Raw = htmlContent;
        Content = MarkdownMarkdig.ToHtml(htmlContent).Replace("\n", "<br>");
    }
}

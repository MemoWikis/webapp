using System.Security.AccessControl;
using TrueOrFalse.Web;

public class InlineTextModel : BaseContentModule
{
    public string Content;
    public string Raw;
    public int Id;
    
    public InlineTextModel(string htmlContent, InlineTextJson json = null)
    {
        Raw = htmlContent;
        if (json == null)
            Content = MarkdownMarkdig.ToHtml(htmlContent).Replace("\n", "<br>");
        else Content = json.Content;
    }

    public InlineTextModel(Category category)
    {
        Id = category.Id;
        Content = category.Content ?? "";
    }
}

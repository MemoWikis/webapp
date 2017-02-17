using System.Web.Mvc;
using TrueOrFalse.Web;

public class MarkdownToHtml
{
    public static string Run(Category category, ControllerContext controllerContext)
    {
        var result = MarkdownMarkdig.ToHtml(category.TopicMarkdown);
        result = TemplateParser.Run(result, category, controllerContext);
        return result;
    }
}

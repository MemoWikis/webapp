using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;

public class TemplateParser
{
    public static string Run(string stringToParse, Category category, ControllerContext controllerContext)
    {
        //Matches "[[something]]" (optionally with surrounding p tag) non-greedily across multiple lines and only if not nested
        var regex = new Regex(@"(<p>)?\[\[(.*?)\]\](<\/p>)?", RegexOptions.Singleline);

        return regex.Replace(stringToParse, match =>
        {
            var templateJson = GetTemplateJson(
                                    match.Value
                                        .Replace("<p>[[","")
                                        .Replace("]]</p>","")
                                        .Replace("[[","")
                                        .Replace("]]","")
                                        .Replace("&quot;", @""""),
                                    category.Id);

            if (templateJson == null)
                return GetReplacementForNonparsableTemplate(match.Value);

            var html = GetHtml(templateJson, category, controllerContext);

            return string.IsNullOrEmpty(html) ? GetReplacementForNonparsableTemplate(match.Value) : html;
        });
    }

    private static TemplateJson GetTemplateJson(string template, int categoryId)
    {
        try
        {
            var templateJson = JsonConvert.DeserializeObject<TemplateJson>(template);
            templateJson.ContainingCategoryId = categoryId;
            return templateJson;
        }

        catch
        {
           return null;
        }
    }

    private static string GetHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
            case "videowidget":
            case "categorynetwork":
            case "contentlists":
            case "singleset":
            case "setlistcard":
            case "singlecategory":
                return GetPartialHtml(templateJson, category, controllerContext);
            default:
                return GetElementHtml(templateJson);
        }
    }

    private static string GetPartialHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext) => 
        GetPartialHtml(templateJson, controllerContext, GetPartialModel(templateJson, category));

    private static string GetPartialHtml(TemplateJson templateJson, ControllerContext controllerContext, BaseModel partialModel)
    {
        try
        {
            return ViewRenderer.RenderPartialView(
                "~/Views/Categories/Detail/Partials/" + templateJson.TemplateName + ".ascx",
                partialModel,
                controllerContext);
        }
        catch
        {
            return null;
        }
    }

    private static BaseModel GetPartialModel(TemplateJson templateJson, Category category)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
                return new TopicNavigationModel(category, templateJson.Title, templateJson.Text, templateJson.TopicIdList, templateJson.OrderType);
            case "videowidget":
                return new VideoWidgetModel(templateJson.SetId);
            case "categorynetwork":
            case "contentlists":
                return new CategoryModel(category, loadKnowledgeSummary : false);
            case "singleset":
                return new SingleSetModel(Sl.R<SetRepo>().GetById(templateJson.SetId), setText: templateJson.SetText);
            case "setlistcard":
                return new SetListCardModel(
                    templateJson.SetList,
                    templateJson.Title,
                    templateJson.Description,
                    category.Id,
                    templateJson.TitleRowCount,
                    templateJson.DescriptionRowCount,
                    templateJson.SetRowCount);
            case "singlecategory":
                return new SingleCategoryModel(
                    templateJson.CategoryId,
                    templateJson.Description);
            default:
                return null;
        }
    }

    private static string GetElementHtml(TemplateJson templateJson)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "divstart":
                return "<div class='" + templateJson.CssClasses + "'>";
            case "divend":
                return "</div>";
            default:
                return null;
        }
    }

    private static string GetReplacementForNonparsableTemplate(string match)//If template cannot be parsed show it for admins, otherwise hide it
    {
        return Sl.R<SessionUser>().IsInstallationAdmin ? "<div style='background-color: rgba(130, 8, 22, 0.33)'>" + match + "</div>" : "";
    }
}
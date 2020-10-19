using System;
using Newtonsoft.Json;

public class TemplateParserForSingleTemplate
{
    public static BaseContentModule Run(Token token, Category category)
    {
        return Run(token.ToText(), category);
    }

    public static BaseContentModule Run(string stringToParse, Category category)
    {
        TemplateJson templateJson;

        if (stringToParse.Contains("[[") && stringToParse.Contains("]]"))
        {
            var json = stringToParse
                .Replace("<p>", "")
                .Replace("</p>", "")
                .Replace("[[", "")
                .Replace("]]", "")
                .Replace("&quot;", @"""");

            templateJson = GetTemplateJson(json);
            templateJson.OriginalJson = json;
        }
        else
        {
            templateJson = new TemplateJson
            {
                TemplateName = "InlineText",
                InlineText = stringToParse
            };
        }

        return GetPartialModel(templateJson, category, stringToParse);
    }

    public static BaseContentModule Run(TemplateJson templateJson, Category category)
    {

        var contentModule = GetPartialModel2(templateJson, category);
        contentModule.TemplateJson = templateJson;
        contentModule.Markdown = "";
        contentModule.Type = templateJson.TemplateName.ToLower();

        return contentModule;
    }

    private static BaseContentModule GetPartialModel(TemplateJson templateJson, Category category, string stringToParse)
    {
        var templateMarkdown = stringToParse
            .Replace("<p>", "")
            .Replace("</p>", "");

        var contentModule = GetPartialModel2(templateJson, category);
        contentModule.TemplateJson = templateJson;
        contentModule.Markdown = templateMarkdown;
        contentModule.Type = templateJson.TemplateName.ToLower();

        return contentModule;
    }

    private static BaseContentModule GetPartialModel2(TemplateJson templateJson, Category category)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
                return new TopicNavigationModel(category, JsonConvert.DeserializeObject<TopicNavigationJson>(templateJson.OriginalJson));
            case "inlinetext":
                return new InlineTextModel(templateJson.InlineText, JsonConvert.DeserializeObject<InlineTextJson>(templateJson.OriginalJson));
            default:
                throw new Exception("Kein Model für diese Template hinterlegt.");
        }
    }

    private static TemplateJson GetTemplateJson(string template)
    {
        return JsonConvert.DeserializeObject<TemplateJson>(template);
    }

}
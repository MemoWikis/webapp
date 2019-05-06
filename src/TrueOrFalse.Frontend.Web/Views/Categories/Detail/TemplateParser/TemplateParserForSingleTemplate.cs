using System;
using Newtonsoft.Json;

public class TemplateParserForSingleTemplate
{
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
            case "medialist":
                return new MediaListModel(category, JsonConvert.DeserializeObject<MediaListJson>(templateJson.OriginalJson));
            case "educationofferlist":
                return new EducationOfferListModel(category, JsonConvert.DeserializeObject<EducationOfferListJson>(templateJson.OriginalJson));
            case "videowidget":
                return new VideoWidgetModel(JsonConvert.DeserializeObject<VideoWidgetJson>(templateJson.OriginalJson));
            case "settestsessionnostartscreen":
                return new SetTestSessionNoStartScreenModel(JsonConvert.DeserializeObject<SetTestSessionNoStartScreenJson>(templateJson.OriginalJson));
            case "singlesetfullwidth":
                return new SingleSetFullWidthModel(JsonConvert.DeserializeObject<SingleSetFullWidthJson>(templateJson.OriginalJson));
            case "singlecategoryfullwidth":
                return new SingleCategoryFullWidthModel(JsonConvert.DeserializeObject<SingleCategoryFullWidthJson>(templateJson.OriginalJson));
            case "categorynetwork":
            case "contentlists":
                return new CategoryModel(category, loadKnowledgeSummary: false);
            case "setlistcard":
                return new SetListCardModel(category.Id, JsonConvert.DeserializeObject<SetListCardJson>(templateJson.OriginalJson));
            case "setcardminilist":
                return new SetCardMiniListModel(JsonConvert.DeserializeObject<SetCardMiniListJson>(templateJson.OriginalJson));
            case "singlecategory":
                return new SingleCategoryModel(JsonConvert.DeserializeObject<SingleCategoryJson>(templateJson.OriginalJson));
            case "singlequestionsquiz":
                return new SingleQuestionsQuizModel(category, JsonConvert.DeserializeObject<SingleQuestionsQuizJson>(templateJson.OriginalJson));
            case "spacer":
                return new SpacerModel(JsonConvert.DeserializeObject<SpacerJson>(templateJson.OriginalJson));
            case "cards":
                return new CardsModel(JsonConvert.DeserializeObject<CardsJson>(templateJson.OriginalJson));
            case "inlinetext":
                return new InlineTextModel(templateJson.InlineText);
            default:
                throw new Exception("Kein Model für diese Template hinterlegt.");
        }
    }

    private static TemplateJson GetTemplateJson(string template)
    {
        return JsonConvert.DeserializeObject<TemplateJson>(template);
    }

}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public class TemplateParser
{
    public static string Run(string stringToParse, Category category, ControllerContext controllerContext)
    {
        //Matches "[[something]]" (optionally with surrounding p tag) non-greedily across multiple lines and only if not nested
        var regex = new Regex(@"(<p>)?\[\[(.*?)\]\](<\/p>)?", RegexOptions.Singleline);

        return regex.Replace(stringToParse, match =>
        {
            try
            {
                var json = match.Value
                    .Replace("<p>[[", "")
                    .Replace("]]</p>", "")
                    .Replace("[[", "")
                    .Replace("]]", "")
                    .Replace("&quot;", @"""");

                var templateJson = GetTemplateJson(json);

                var templateMarkdown = match.Value
                    .Replace("<p>", "")
                    .Replace("</p>", "");

                templateJson.OriginalJson = json;

                var html = GetHtml(
                    templateJson, 
                    category, 
                    controllerContext, 
                    templateMarkdown
                );

                if (string.IsNullOrEmpty(html))
                    throw new Exception("Es konnte kein Html erzeugt werden.");

                return html;

            }
            catch (Exception e)
            {
                Logg.r().Error($"Fehler beim Parsen der Kategorie Id={category.Id} ({e.Message} {e.StackTrace}).");
                return GetReplacementForNonparsableTemplate(match.Value, e.Message);
            }

        });
    }

    private static TemplateJson GetTemplateJson(string template)
    {
        return JsonConvert.DeserializeObject<TemplateJson>(template);
    }

    private static string GetHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext, string templateMarkdown)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
            case "medialist":
            case "videowidget":
            case "settestsessionnostartscreen":
            case "singlesetfullwidth":
            case "singlecategoryfullwidth":
            case "categorynetwork":
            case "contentlists":
            case "educationofferlist":
            case "setlistcard":
            case "setcardminilist":
            case "singlecategory":
            case "singlequestionsquiz":
            case "spacer":
            case "textblock":
            case "cards":
                return GetPartialHtml(templateJson, category, controllerContext, templateMarkdown);
            default:
            {
                var elementHtml = GetElementHtml(templateJson);

                if (string.IsNullOrEmpty(elementHtml))
                    throw new Exception($"Name des Templates '{templateJson.TemplateName}' ist unbekannt.");

                return elementHtml;
            }
        }
    }

    private static string GetPartialHtml(
        TemplateJson templateJson, 
        Category category,
        ControllerContext controllerContext, 
        string templateMarkdown)
    {
        var partialModel = GetPartialModel(templateJson, category);
        partialModel.Markdown = templateMarkdown;

        return GetPartialHtml(templateJson, controllerContext, partialModel);
    }

    private static string GetPartialHtml(TemplateJson templateJson, ControllerContext controllerContext, BaseModel partialModel)
    {
        return ViewRenderer.RenderPartialView(
            $"~/Views/Categories/Detail/Partials/{templateJson.TemplateName}/{templateJson.TemplateName}.ascx",
            partialModel,
            controllerContext);
    }

    private static BaseContentModule GetPartialModel(TemplateJson templateJson, Category category)
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
                return  new SingleCategoryFullWidthModel(JsonConvert.DeserializeObject<SingleCategoryFullWidthJson>(templateJson.OriginalJson));
            case "categorynetwork":
            case "contentlists":
                return new CategoryModel(category, loadKnowledgeSummary : false);
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
            case "textblock":
                return new TextBlockModel(JsonConvert.DeserializeObject<TextBlockJson>(templateJson.OriginalJson));
            case "cards":
                return new CardsModel(JsonConvert.DeserializeObject<CardsJson>(templateJson.OriginalJson));
            default:
                throw new Exception("Kein Model für diese Template hinterlegt.");
        }
    }

    private static string GetElementHtml(TemplateJson templateJson)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "wishknowledgeinthebox":
                return "<div class = 'wishKnowledgeTemplate'></div>";
            default:
                return null;
        }
    }

    /// <summary>
    /// /If template cannot be parsed show it for admins, otherwise hide it
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private static string GetReplacementForNonparsableTemplate(string match, string exceptionMessage)
    {
        return Sl.SessionUser.IsInstallationAdmin 
            ? $"<div style=\'background-color: rgba(130, 8, 22, 0.33); margin-bottom: 20px;\'>Folgendes Template konnte nicht umgewandelt werden:<div>{match}</div>Fehler: {exceptionMessage}</div>"
            : "";
    }
}
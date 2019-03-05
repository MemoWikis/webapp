using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class MarkdownToHtml
{
    public static string Run(string markdown, Category category, ControllerContext controllerContext)
    {
        if(String.IsNullOrEmpty(markdown?.Trim()))
            return "";

//        var result = MarkdownMarkdig.ToHtml(markdown);
//        result = TemplateParser.Run(result, category, controllerContext);

        var result = TurnPartsIntoHtml(markdown, category, controllerContext);
        return result;
    }

    public static string TurnPartsIntoHtml(string markdown, Category category, ControllerContext controllerContext)
    {
        var parts = SplitMarkdown(markdown);
        var result = new StringBuilder();
        var textWrapperStart = "[[{\"TemplateName\" : \"InlineText\" , \"Content\" : \"";
        var textWrapperEnd = "\"}]]";
        foreach (Part element in parts)
        {
            var html = "";
//            var textElement = textWrapperStart + element.ToText() + textWrapperEnd;
            var textContent = element.ToText();
            html = element.Type == PartType.Text ? textContent : MarkdownMarkdig.ToHtml(element.ToText());

            var htmlResult = TemplateParser.Run(html, category, controllerContext);

            result.Append(htmlResult);
        }

        return result.ToString();
    }

    private static string GetPartialInlineHtml(ControllerContext controllerContext, BaseModel partialModel)
    {
        return ViewRenderer.RenderPartialView(
            $"~/Views/Categories/Detail/Partials/InlineText/InlineText.ascx",
            partialModel,
            controllerContext);
    }


    public static List<Part> SplitMarkdown(string markdown)
    {
        var parts = new List<Part>();
        var currentPart = new Part();
        string inputText = markdown;
        char lastChar = ' ';
        char preLastChar = ' ';

        var chars = inputText.ToCharArray();
        var charsLength = chars.Length;
        for (var i = 0; i < charsLength; i++ )
        {
            var character = chars[i];
            var hasNextChar = i < charsLength - 1;

            char nextChar = ' ';
            if (hasNextChar)
                nextChar = chars[i + 1];
            if (!hasNextChar)
                parts.Add(currentPart);

            if (character == '[' && nextChar == '[')
            {
               if (currentPart.ToText().Length > 0)
                    parts.Add(currentPart);

                currentPart = new Part { Type = PartType.Template };
            }

            if (preLastChar == ']' && lastChar == ']' && character != '[' && nextChar != '[')
            {
                parts.Add(currentPart);
                currentPart = new Part { Type = PartType.Text };
            }

            preLastChar = lastChar;
            lastChar = character;

            currentPart.AddChar(character);

        }

        return parts;
    }
}

public enum PartType
{
    Text,
    Template
};

public class Part
{
    public PartType Type;

    private StringBuilder _sb = new StringBuilder();

    public bool IsTemplate => Type == PartType.Template;
    public bool IsText => Type == PartType.Text;

    public void AddChar(char character) => _sb.Append(character);
    public string ToText() => _sb.ToString();
}

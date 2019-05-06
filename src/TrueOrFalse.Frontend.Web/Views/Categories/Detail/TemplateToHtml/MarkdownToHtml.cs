using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

public class MarkdownToHtml
{
    public static string Run(string markdown, Category category, ControllerContext controllerContext)
    {
        if(String.IsNullOrEmpty(markdown?.Trim()))
            return "";

        var result = MarkdownContentToHtml(markdown, category, controllerContext);
        return result;
    }

    public static string MarkdownContentToHtml(string markdown, Category category, ControllerContext controllerContext)
    {
        var parts = SplitMarkdown(markdown);
        var result = new StringBuilder();
        foreach (Part element in parts)
        {
            var htmlResult = MarkdownSingleTemplateToHtml.Run(element, category, controllerContext);

            result.Append(htmlResult);
        }

        return result.ToString();
    }

    public static List<Part> SplitMarkdown(string markdown)
    {
        var parts = new List<Part>();
        var currentPart = new Part();
        string inputText = markdown;
        char lastChar = ' ';
        char preLastChar = ' ';
        string previousPart = "";

        if (inputText.Trim().StartsWith("[["))
            currentPart.Type = PartType.Template;

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
                if (currentPart.ToText().Trim().Length > 0)
                {
                    parts.Add(currentPart);
                    previousPart = currentPart.ToText();
                    if (previousPart.EndsWith("]]"))
                    {
                        currentPart = new Part { Type = PartType.Text };
                        currentPart.AddNewLine();
                        parts.Add(currentPart);
                        previousPart = currentPart.ToText();
                    }

                    currentPart = new Part { Type = PartType.Template };
                }
                else if (currentPart.ToText().Trim().Length == 0 && previousPart.EndsWith("]]"))
                {
                    currentPart = new Part { Type = PartType.Text };
                    currentPart.AddNewLine();
                    parts.Add(currentPart);
                    previousPart = currentPart.ToText();

                    currentPart = new Part { Type = PartType.Template };
                }
                else
                    currentPart = new Part { Type = PartType.Template };
            }
            else if (preLastChar == ']' && lastChar == ']' && !(character == '[' && nextChar == '['))
            {
                parts.Add(currentPart);
                previousPart = currentPart.ToText();
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

    private readonly StringBuilder _sb = new StringBuilder();

    public bool IsTemplate => Type == PartType.Template;
    public bool IsText => Type == PartType.Text;

    public void AddChar(char character) => _sb.Append(character);
    public void AddNewLine() => _sb.Append("\r\n");
    public string ToText() => _sb.ToString();
}

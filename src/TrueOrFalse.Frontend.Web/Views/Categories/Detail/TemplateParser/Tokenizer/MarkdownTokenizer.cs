using System.Collections.Generic;

static internal class MarkdownTokenizer
{
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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TemplateMigration
{
    internal static class MarkdownConverter
    {
        public static string ConvertMarkdown(string topicMarkdown)
        {
            var parts = SplitMarkdown(topicMarkdown);
            
            string newMarkdown = "";
            if (topicMarkdown.Contains("DivStart"))
            {
                var currentTemplateType = PartType.None;

                foreach (var part in parts)
                {
                    if (part.IsTemplate && (part.Contains("box") || part.Contains("centerelements")))
                    {
                        currentTemplateType = PartType.Text;
                    }
                    else if (part.IsTemplate && part.Contains("cards"))
                    {
                        currentTemplateType = PartType.Template;

                        newMarkdown += "[[{\"TemplateName\":\"Cards\",";

                        if (part.Contains("potrait"))
                            newMarkdown += "\"CardOrientation\":\"Portrait\",";
                        else if (part.Contains("landscape"))
                            newMarkdown += "\"CardOrientation\":\"Landscape\",";

                        newMarkdown += "\"SetListIds\":\"";
                    }
                    else if (part.IsTemplate && part.Contains("singleset"))
                    {
                        var setIdValue = Regex.Match(part.ToString(), @"\d+").Value;
                        newMarkdown += (setIdValue + ",");
                    }
                    else if (part.IsTemplate && part.Contains("divend"))
                    {
                        if (currentTemplateType == PartType.Template)
                            newMarkdown += "\"}]]";
                        else if (currentTemplateType == PartType.Text)
                            currentTemplateType = PartType.Unchanged;
                    }
                    else
                    {
                        newMarkdown += part;
                    }
                }
            }

            return newMarkdown;
        }

        private static List<Part> SplitMarkdown(string markdown)
        {
            var parts = new List<Part>();
            var currentPart = new Part();
            string inputText = markdown;
            char lastChar = ' ';
            char preLastChar = ' ';

            var chars = inputText.ToCharArray();
            var charsLength = chars.Length;
            for (var i = 0; i < charsLength; i++)
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

}
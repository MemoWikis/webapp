using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TemplateMigration
{
    public static class MarkdownConverter
    {
        public static string ConvertMarkdown(string topicMarkdown)
        {
            if (!topicMarkdown.Contains("DivStart"))
                return topicMarkdown;

            var parts = SplitMarkdown(topicMarkdown);
            var currentTemplateType = PartType.None;
            string newMarkdown = "";
            foreach (var part in parts)
            {
                if (part.IsTemplate && (part.Contains("box") || part.Contains("centerelements")))
                {
                    currentTemplateType = PartType.Text;
                }
                else if (part.IsTemplate && part.Contains("row") && part.Contains("cards"))
                {
                    currentTemplateType = PartType.Template;

                    newMarkdown += "[[{\"TemplateName\":\"Cards\",";

                    if (part.Contains("portrait"))
                        newMarkdown += "\"CardOrientation\":\"Portrait\",";
                    else
                        newMarkdown += "\"CardOrientation\":\"Landscape\",";

                    newMarkdown += "\"SetListIds\":\"";
                }
                else if (part.IsTemplate && part.Contains("singleset"))
                {
                    var setIdValue = Regex.Match(part.ToText(), @"\d+").Value;
                    newMarkdown += (setIdValue + ",");
                }
                else if (part.IsTemplate && part.Contains("divend"))
                {
                    if (currentTemplateType == PartType.Template)
                    {
                        newMarkdown = newMarkdown.Remove(newMarkdown.Length - 1);
                        newMarkdown += "\"}]]";
                    }
                    else if (currentTemplateType == PartType.Text)
                        currentTemplateType = PartType.None;
                }
                else
                {
                    newMarkdown += part.ToText();
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

            if (inputText.Trim().StartsWith("[["))
                currentPart.Type = PartType.Template;

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
                } else if (preLastChar == ']' && lastChar == ']' )
                {
                    parts.Add(currentPart);

                    if (character == '[' && nextChar == '[')
                        currentPart = new Part { Type = PartType.Template };
                    else
                        currentPart = new Part {Type = PartType.Text};
                }

                preLastChar = lastChar;
                lastChar = character;

                currentPart.AddChar(character);

            }

            return parts;
        }
    }

}
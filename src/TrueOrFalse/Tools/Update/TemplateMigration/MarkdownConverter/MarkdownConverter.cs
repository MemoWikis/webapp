using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TemplateMigration
{
    public static class MarkdownConverter
    {
        public static string ConvertMarkdown(string topicMarkdown)
        {
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
                else if (part.IsTemplate && part.Contains("singleset") && !(part.Contains("fullwidth")))
                {
                    var setIdValue = Regex.Match(part.ToText(), @"\d+").Value;
                    newMarkdown += (setIdValue + ",");
                }
                else if (part.IsTemplate && part.Contains("divend"))
                {
                    if (currentTemplateType == PartType.Template)
                    {
                        newMarkdown = newMarkdown.Remove(newMarkdown.Length - 1); //removes last comma on SetListIds
                        newMarkdown += "\"}]]";
                        currentTemplateType = PartType.None;
                    }
                    else if (currentTemplateType == PartType.Text)
                        currentTemplateType = PartType.None;
                }
                else if (currentTemplateType != PartType.Template)
                        newMarkdown += part.ToText();
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
            string previousPart = "";

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
                    } else if (currentPart.ToText().Trim().Length == 0 && previousPart.EndsWith("]]"))
                    {
                        currentPart = new Part { Type = PartType.Text };
                        currentPart.AddNewLine();
                        parts.Add(currentPart);
                        previousPart = currentPart.ToText();

                        currentPart = new Part { Type = PartType.Template };
                    } else
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

        public static string ConvertTemplates(string topicMarkdown)
        {
            var parts = SplitMarkdown(topicMarkdown);
            string newMarkdown = "";

            foreach (var part in parts)
            {
                if (!part.Contains("[["))
                    part.Type = PartType.Text;

                if (part.IsTopicNavigation || part.IsText || part.IsCategoryNetwork)
                {
                    newMarkdown += part.ToText();
                }
                else if (part.Contains("\"cards\"") || part.Contains("\"singlesetfullwidth\"") || part.Contains("\"setCardMiniList\""))
                {
                    var baseString = part.ToText();
                    var searchTerm = "";
                    if (part.Contains("\"cards\""))
                        searchTerm = "SetIds\":(.*)";
                    else if (part.Contains("\"singlesetfullwidth\""))
                        searchTerm = "SetId\":(.*)";
                    else if (part.Contains("\"setCardMiniList\""))
                        searchTerm = "SetListIds\":(.*)";

                    var rxAfterSetId = new Regex(searchTerm);
                    var stringAfterSetId = rxAfterSetId.Match(baseString).ToString();

                    var rxBeforeFirstQuotation = new Regex(@"^[^""]+");
                    var stringBeforeQuotation = rxBeforeFirstQuotation.Match(stringAfterSetId).ToString();

                    var rxNumbers = new Regex(@"\D+");
                    var numbersStringArray = rxNumbers.Split(stringBeforeQuotation);

                    var newNumbers = new List<int>();

                    foreach (var setIdString in numbersStringArray)
                    {
                        if (!string.IsNullOrEmpty(setIdString))
                        {
                            var setId = int.Parse(setIdString);
                            var category = Sl.CategoryRepo.GetBySetId(setId);
                            var categoryChanges = Sl.CategoryChangeRepo.GetForCategory(category.Id);
                            var isDeleted = categoryChanges.Any(c => c.Category == category && c.Type == CategoryChangeType.Delete);
                            if (isDeleted)
                            {
                                var baseSetId = Sl.SetRepo.GetById(setId).CopiedFrom.Id;
                                category = Sl.CategoryRepo.GetBySetId(baseSetId);
                            }

                            newNumbers.Add(category.Id);
                        }
                    }


                    newMarkdown += "[[{\"TemplateName\":\"TopicNavigation\", \"Load\":" + string.Join(", ", newNumbers) + "}]]";
                }
                else if (part.Contains("\"singlecategoryfullwidth\""))
                {
                    var rx = new Regex(@"(\d +)(?!.*\d)");
                    var categoryId = rx.Match(part.ToText());

                    newMarkdown += "[[{\"TemplateName\":\"TopicNavigation\", \"Load\":" + categoryId + "}]]";
                }
            }

            return newMarkdown.TrimStart().TrimEnd();
        }
    }

}
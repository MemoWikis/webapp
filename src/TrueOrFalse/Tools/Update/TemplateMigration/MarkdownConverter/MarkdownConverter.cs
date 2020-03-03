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
                    var markDown = part.ToText();
                    if (part.IsTopicNavigation || part.IsTopicNavigation)
                        markDown = part.ToText().Replace("&quot;", "\"");
                    newMarkdown += markDown;
                }
                else if (part.Contains("\"cards\"") || 
                         part.Contains("\"singlesetfullwidth\"") || 
                         part.Contains("\"setCardMiniList\"") || 
                         part.Contains("\"singlecategoryfullwidth\"") || 
                         part.Contains("\"setlistcard\""))
                {
                    var baseString = part.ToText();
                    var searchTerm = "";
                    var isCategory = false;
                    if (part.Contains("\"cards\""))
                        searchTerm = "(?i)SetIds\":(.*)";
                    else if (part.Contains("\"singlesetfullwidth\""))
                        searchTerm = "(?i)SetId\":(.*)";
                    else if (part.Contains("\"setcardminilist\"") || part.Contains("\"setlistcard\""))
                        searchTerm = "(?i)SetListIds\":(.*)";
                    else if (part.Contains("\"singlecategoryfullwidth\""))
                    {
                        searchTerm = "(?i)CategoryId\":(.*)";
                        isCategory = true;
                    }

                    var replaceFirstQuotation = new Regex("\"");

                    var title = "";
                    if (part.Contains("\"title\":"))
                    {
                        var titleSearchTerm = "(?i)title\":(.*)";
                        var rxTitle = new Regex(titleSearchTerm);
                        var titleString = rxTitle.Match(baseString).Groups[1].ToString();
                        var titleText = replaceFirstQuotation.Replace(titleString, "", 1);
                        if (titleText.Trim().EndsWith("\",\""))
                            titleText = titleText.Remove(titleText.Length - 1, 1);
                        else if (!titleText.Trim().EndsWith("\","))
                            titleText += "\",";
                        title =  "\"Title\":\"" + titleText;
                    }

                    var description = "";
                    if (part.Contains("\"description\":") || part.Contains("\"text\":"))
                    {
                        var descriptionSearchTerm = "(?i)description\":(.*)";
                        if (part.Contains("\"text\":"))
                            descriptionSearchTerm = "(?i)text\":(.*)";

                        var rxDescription = new Regex(descriptionSearchTerm);
                        var titleString = rxDescription.Match(baseString).Groups[1].ToString();
                        var descriptionText = replaceFirstQuotation.Replace(titleString, "", 1);
                        if (descriptionText.Trim().EndsWith("\",\""))
                            descriptionText = descriptionText.Remove(descriptionText.Length -1, 1);
                        else if (!descriptionText.Trim().EndsWith("\","))
                            descriptionText += "\",";
                        description = "\"Text\":\"" + descriptionText;
                    }

                    var rxAfterId = new Regex(searchTerm);
                    var stringAfterId = rxAfterId.Match(baseString).Groups[1].ToString();
                    if (stringAfterId.Trim().StartsWith("\""))
                        stringAfterId = replaceFirstQuotation.Replace(stringAfterId, "",1);

                    var rxBeforeFirstQuotation = new Regex(@"^[^""]+");
                    var stringBeforeQuotation = rxBeforeFirstQuotation.Match(stringAfterId).ToString();

                    var rxNumbers = new Regex(@"\D+");
                    var numbersStringArray = rxNumbers.Split(stringBeforeQuotation);

                    var ids = new List<int>();

                    foreach (var idString in numbersStringArray)
                    {
                        if (!string.IsNullOrEmpty(idString))
                        {
                            var id = int.Parse(idString);
                            if (isCategory)
                                ids.Add(id);
                            else
                            {
                                var category = Sl.CategoryRepo.GetBySetId(id);
                                var categoryChanges = Sl.CategoryChangeRepo.GetForCategory(category.Id);
                                var isDeleted = categoryChanges.Any(c => c.Category == category && c.Type == CategoryChangeType.Delete);
                                if (isDeleted)
                                {
                                    var baseSetId = Sl.SetRepo.GetById(id).CopiedFrom.Id;
                                    category = Sl.CategoryRepo.GetBySetId(baseSetId);
                                }

                                ids.Add(category.Id);
                            }
                        }
                    }

                    var loadString = "\"Load\":\"" + string.Join(", ", ids) + "\"";

                    if (ids != null)
                        newMarkdown += "[[{\"TemplateName\":\"TopicNavigation\", "+ title + description + loadString + "}]]";
                }
            }

            return newMarkdown.TrimStart().TrimEnd();
        }
    }

}
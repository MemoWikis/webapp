using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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
                if (!part.Contains("[[") && !part.Contains("]]"))
                    newMarkdown += part.ToText();

                else
                {
                    if (part.Contains("[[") && part.Contains("]]"))
                    {
                        var jsonString = part.ToText()
                            .Replace("<p>", "")
                            .Replace("</p>", "")
                            .Replace("[[", "")
                            .Replace("]]", "")
                            .Replace("&quot;", @"""");

                        var json = JsonConvert.DeserializeObject<TemplateJson>(jsonString);

                        if (json.TemplateName.ToLower() == "topicnavigation" || json.TemplateName.ToLower() == "categorynetwork")
                        {
                            newMarkdown += part.ToText();
                            continue;
                        }

                        var title = "";
                        var text = "";

                        if (!string.IsNullOrEmpty(json.Title))
                            title = "\"Title\":\"" + json.Title + "\",";

                        if (!string.IsNullOrEmpty(json.Description))
                            text = "\"Text\":\"" + json.Description + "\",";
                        else if (!string.IsNullOrEmpty(json.Text))
                            text = "\"Text\":\"" + json.Text + "\",";

                        var load = "\"Load\":\"";
                        var rxNumbers = new Regex(@"\D+");

                        if (json.TemplateName.ToLower() == "singlecategoryfullwidth")
                        {
                            var numbersStringArray = rxNumbers.Split(json.CategoryId);
                            var parsedId = int.Parse(numbersStringArray[0]);
                            load += parsedId + "\"";
                            newMarkdown += "[[{\"TemplateName\":\"TopicNavigation\", " + title + text + load + "}]]";
                        } else if (json.TemplateName.ToLower() == "cards" ||
                            json.TemplateName.ToLower() == "singlesetfullwidth" ||
                            json.TemplateName.ToLower() == "setcardminilist" ||
                            json.TemplateName.ToLower() == "setlistcard")
                        {
                            var ids = new List<int>();
                            var idsString = "";
                            if (!string.IsNullOrEmpty(json.SetId))
                                idsString = json.SetId;
                            else if (!string.IsNullOrEmpty(json.SetIds))
                                idsString = json.SetIds;
                            else if (!string.IsNullOrEmpty(json.SetListIds))
                                idsString = json.SetListIds;

                            var numbersStringArray = rxNumbers.Split(idsString);
                            foreach (var idString in numbersStringArray)
                            {
                                if (!string.IsNullOrEmpty(idString)) {
                                    var id = int.Parse(idString);
                                    var category = Sl.CategoryRepo.GetBySetId(id);
                                    if (category == null)
                                        continue;

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

                            load += string.Join(", ", ids) + "\"";
                            if (ids.Count >= 1)
                                newMarkdown += "[[{\"TemplateName\":\"TopicNavigation\", " + title + text + load + "}]]";
                        }
                    }
                }
            }

            return newMarkdown.TrimStart().TrimEnd();
        }
    }

    public class TemplateJson
    {
        public string TemplateName;
        public string Title;
        public string Text;
        public string Description;
        public string CategoryId;
        public string SetId;
        public string SetIds;
        public string SetListIds;
    }

}
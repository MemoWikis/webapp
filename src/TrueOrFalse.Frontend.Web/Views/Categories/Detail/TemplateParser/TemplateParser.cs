using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class TemplateParser
{
    public static Document Run(string markdownString, Category category)
    {
        var tokens = MarkdownTokenizer.Run(markdownString);
        var elements = tokens
            .Select(token => TemplateParserForSingleTemplate.Run(token, category))
            .ToList();

        return new Document(elements);
    }

    public static Document Run2(string contentString, Category category)
    {
        var tokens = Tokenizer.Run(contentString);

        var elements = tokens
            .Select(token => TemplateParserForSingleTemplate.Run(token, category))
            .ToList();

        return new Document(elements);
    }

    public static string GetContent(List<JsonLoader> content)
    {
        if (content == null) //page is empty
            return "";

        var mergedContent = new List<JsonLoader>();
        var currentIsText = false;
        var currentText = "";

        foreach (var module in content)
        {
            if (module.TemplateName == "TopicNavigation")
            {
                if (currentIsText && !string.IsNullOrEmpty(currentText))
                {
                    var inlineTextModule = new JsonLoader
                    {
                        TemplateName = "InlineText",
                        Content = currentText
                    };
                    mergedContent.Add(inlineTextModule);
                    currentText = "";
                }

                mergedContent.Add(module);
                currentIsText = false;
            }
            else if (module.TemplateName == "InlineText")
            {
                currentText += module.Content;
                currentIsText = true;
            }
        }

        if (!string.IsNullOrEmpty(currentText))
        {
            var inlineTextModule = new JsonLoader
            {
                TemplateName = "InlineText",
                Content = currentText
            };
            mergedContent.Add(inlineTextModule);
        }

        return JsonConvert.SerializeObject(mergedContent, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }

    public class JsonLoader
    {
        public string TemplateName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Load { get; set; }
        public string Order { get; set; }
        public string Content { get; set; }
    }
}

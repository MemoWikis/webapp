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
        var decoded = System.Net.WebUtility.HtmlDecode(contentString);
        dynamic jsonObject = JsonConvert.DeserializeObject(decoded);
        var tokens = new List<TemplateJson>();

        foreach (var obj in jsonObject)
        {
            var json = JsonConvert.DeserializeObject<TemplateJson>(obj.Value);
            json.OriginalJson = obj.Value; ;
            tokens.Add(json);
        }

        var elements = tokens
            .Select(token => TemplateParserForSingleTemplate.Run(token, category))
            .ToList();

        return new Document(elements);
    }

}

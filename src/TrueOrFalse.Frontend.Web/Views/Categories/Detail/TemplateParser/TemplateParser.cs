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
}

using System.Collections.Generic;
using System.Linq;

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
}

using System.Collections.Generic;
using System.Linq;

public class TemplateParser
{
    public static IList<BaseContentModule> Run(string markdownString, Category category)
    {
        var tokens = MarkdownTokenizer.Run(markdownString);
        return tokens
            .Select(token => TemplateParserForSingleTemplate.Run(token, category))
            .ToList();
    }

}

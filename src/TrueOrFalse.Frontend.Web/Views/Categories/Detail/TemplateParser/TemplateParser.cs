using System.Collections.Generic;
using System.Linq;

public class TemplateParser
{
    public static IList<BaseContentModule> Run(string markdownString, Category category)
    {
        var parts = MarkdownTokenizer.Run(markdownString);
        return parts
            .Select(part => TemplateParserForSingleTemplate.Run(part, category))
            .ToList();
    }

}

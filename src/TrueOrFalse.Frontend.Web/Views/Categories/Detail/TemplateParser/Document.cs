using System.Collections.Generic;
using System.Linq;

public class Document
{
    public IList<BaseContentModule> Elements;

    public Document(IList<BaseContentModule> elements)
    {
        Elements = elements;
    }

    public string GetDescription(Document elements)
    {
        var firstInlineTextElement = elements.Elements.Select(token => token.Type == "inlinetext").First();
        var description = "";

        return description;
    }
}

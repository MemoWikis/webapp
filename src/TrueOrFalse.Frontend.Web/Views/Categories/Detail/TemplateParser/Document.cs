using System;
using System.Collections.Generic;
using System.Linq;

public class Document
{
    public IList<BaseContentModule> Elements;

    public Document(IList<BaseContentModule> elements)
    {
        Elements = elements;
    }

    public static string GetDescription(Document elements)
    {
        var element = elements.Elements;
        var textElements = element.Where(e => e.IsText).Select(e => e.Markdown.Trim()).ToList();
        var inlineTexts = String.Join("\r\n", textElements);
        string trimmedText = inlineTexts.Trim();

        char[] array = trimmedText.Take(150).ToArray();
        string shortenedText = new string(array);

        string description = shortenedText;

        if (trimmedText.Length > 150)
            description = description + "...";

        return description;
    }
}

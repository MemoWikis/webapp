using System.Text;
using System.Text.RegularExpressions;

public class ParseTemplate
{
    public static Template GetTemplateByName(string markup, string[] templateNames)
    {
        foreach (var templateName in templateNames)
        {
            var template = GetTemplateByName(markup, templateName);

            if (template.IsSet)
                return template;
        }

        return new Template("", templateNames.Aggregate((a, b) => a + " " + b));
    }

    public static Template GetTemplateByName(string markup, string templateName)
    {
        string[] markupTokenized = TokenizeMarkup(markup);

        bool collect = false;
        var sbCollected = new StringBuilder();
        int indent = 0;
        var previousToken = "";
        foreach (var token in markupTokenized)
        {
            if (token == "{{")
                indent++;

            if (token == "}}")
                indent--;

            if (previousToken + token == "{{" + templateName)
            {
                collect = true;
                continue;
            }

            //$temp: müsste nicht hier indent nicht eher wie bei Beginn des Templates sein (oder bei Beginn auch 0)?
            if (token == "}}" && indent == 0)
                collect = false;

            if (collect)
                sbCollected.Append(token);

            previousToken = token;
        }

        return new Template(sbCollected.ToString(), templateName);
    }

    public static List<Template> GetAllMatchingTemplates(
        string markup,
        List<string> templateNames)
    {
        string[] markupTokenized = TokenizeMarkup(markup);

        bool collect = false;
        var templateToken = "";
        var sbCollected = new StringBuilder();
        var parsedTemplates = new List<Template>();
        int indent = 0;
        var previousToken = "";
        foreach (var token in markupTokenized)
        {
            if (token == "{{")
                indent++;

            if (token == "}}")
                indent--;

            if (templateNames.Any(templateName => previousToken + token == "{{" + templateName))
            {
                templateToken = token;
                collect = true;
                continue;
            }

            if (collect && token == "}}" && indent == 0)
            {
                collect = false;
                parsedTemplates.Add(new Template(sbCollected.ToString(), templateToken));
                sbCollected = new StringBuilder();
            }

            if (collect)
                sbCollected.Append(token);

            //$temp: müsste das nicht oben stehen in den Fällen, in denen der Schleifendurchlauf unterbrochen wird?
            previousToken = token;
        }

        return parsedTemplates;
    }

    public static List<Template> GetParameterSubtemplates(Parameter parameter)
    {
        string[] markupTokenized = TokenizeMarkup(parameter.Value);

        bool collectRawText = false;
        bool collectToken = false;
        var rawTextCollected = new StringBuilder();
        var tokenCollected = new StringBuilder();
        var parsedTemplates = new List<Template>();
        int indent = 0;
        foreach (var token in markupTokenized)
        {
            if (token == "{{")
            {
                indent++;

                if (collectToken)
                {
                    collectToken = false;
                    collectRawText = true;
                    continue;
                }

                if (indent == 1)
                {
                    collectToken = true;
                    continue;
                }
            }

            if (token == "|")
            {
                if (collectToken)
                {
                    collectToken = false;
                    collectRawText = true;
                }
            }

            if (token == "}}")
            {
                indent--;

                if (collectToken)
                {
                    collectToken = false;
                    collectRawText = false;
                    parsedTemplates.Add(new Template(tokenCollected.ToString(),
                        "")); //sic: what seemed to be token must be raw text, no token present
                    rawTextCollected = new StringBuilder();
                    tokenCollected = new StringBuilder();
                    continue;
                }

                if (indent == 0 && collectRawText)
                {
                    collectRawText = false;
                    parsedTemplates.Add(new Template(rawTextCollected.ToString(),
                        tokenCollected.ToString()));
                    rawTextCollected = new StringBuilder();
                    tokenCollected = new StringBuilder();
                    continue;
                }
            }

            if (collectRawText)
                rawTextCollected.Append(token);

            if (collectToken)
                tokenCollected.Append(token);
        }

        return parsedTemplates;
    }

    public static string[] TokenizeMarkup(string markup)
    {
        return String.IsNullOrEmpty(markup)
            ? new string[] { }
            : (Regex.Split(markup, "({{|}}|\\r|\\n|\\|)"));
    }
}
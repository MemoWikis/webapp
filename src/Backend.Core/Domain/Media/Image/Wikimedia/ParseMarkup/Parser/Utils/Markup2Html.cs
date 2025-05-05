using System.Text.RegularExpressions;

public class Markup2Html
{
    public static string TransformAll(string markup)
    {
        markup = TransformFormattingMarkup(markup);

        markup = TransformLinkMarkup(markup);

        return markup;
    }

    public static string TransformFormattingMarkup(string markup)
    {
        //Overview wikimedia markup formatting: https://www.mediawiki.org/wiki/Help:Formatting

        //finds "''''displayName''''" transforms to "<i><b>displayName</b></i>"
        markup = Regex.Replace(markup, "''''(.*?)''''",
            match => "<i><b>" + match.Groups[1].Value + "</b></i>");
        //finds "'''displayName'''" transforms to "<b>displayName</b>"
        markup = Regex.Replace(markup, "'''(.*?)'''",
            match => "<b>" + match.Groups[1].Value + "</b>");
        //finds "''displayName''" transforms to "<i>displayName</i>"
        markup = Regex.Replace(markup, "''(.*?)''",
            match => "<i>" + match.Groups[1].Value + "</i>");

        return markup;
    }

    public static string TransformLinkMarkup(string markup)
    {
        //Overview wikimedia markup links: https://www.mediawiki.org/wiki/Help:Links

        //finds "[[User: userName|displayName]]" transforms to "displayName" resp. "displayName (User: userName)" if different
        //http://commons.wikimedia.org/wiki/File:20100723_liege07.JPG
        //|Author=[[User:Jeanhousen|Jean Housen]]
        markup = Regex.Replace(markup, "\\[\\[User:(.*?)\\|(.*?)\\]\\]",
            match => match.Groups[1].Value == match.Groups[2].Value
                ? match.Groups[2].Value
                : match.Groups[2].Value + " (User: " + match.Groups[1].Value + ")"
        );

        //finds "[[:de:pageName|displayName]]" (link to wikipedia article in indicated language, displayed as "de: displayName") transforms to "displayName"
        //and "[[de:pageName|displayName]]" (link to wikipedia article in indicated language, displayed as "displayName") transforms to "displayName"
        foreach (var languageToken in WikiLanguage.GetAllLanguages()
                     .Select(x => x.LanguageToken))
        {
            markup = Regex.Replace(markup, "\\[\\[:?" + languageToken + ":(.*?)\\|(.*?)\\]\\]",
                match => (languageToken == "de" || languageToken == "en")
                    ? match.Groups[2].Value + " (http://" + languageToken +
                      ".wikipedia.org/wiki/" + match.Groups[1].Value.Replace(' ', '_') + ")"
                    : match.Groups[2].Value);
        }

        //finds "[http://link.com  diplayName]" transforms to "displayName, http://link.com"
        markup = Regex.Replace(markup, "\\[(http://[\\S]*)\\s(.*)\\]",
            match => match.Groups[2].Value + ", " + match.Groups[1]);

        //finds "[http://link.com]" (only without spaces and not in [[displayName]]) (numbered external link, displayed on wiki as "[1]") transforms to "displayName"
        markup = Regex.Replace(markup, "(?<!\\[)\\[([^\\[\\])]*?)\\](?!\\])",
            match => match.Groups[1].Value);

        //not handled so far:

        //finds "[[pageName|displayName]]" (other internal piped link)
        //markup = Regex.Replace(markup, "\\[\\[.*?\\|(.*?)\\]\\]", match => match.Groups[1].Value);
        //$temp: vielleicht zu allgemein, nicht alle internen Links machen Sinn als reiner Text (z.B. "User talk": [[User:Ralf Roletschek|Ralf Roletschek]] ([[User talk:Ralf Roletschek|<span class="signature-talk">talk</span>]])")

        //finds "[[displayName]]" (internal link) transforms to "displayName"
        //markup = Regex.Replace(markup, "\\[\\[(.*?)\\]\\]", match => match.Groups[1].Value);

        return markup;
    }
}
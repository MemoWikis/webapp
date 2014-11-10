using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseTemplate
    {
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

                if (token == "}}" && indent == 0)
                    collect = false;

                if (collect)
                    sbCollected.Append(token);

                previousToken = token;
            }

            return new Template(sbCollected.ToString(), templateName);
        }

        public static string GetFirstMatchingTemplate(string markup, List<string> templateNames)
        {
            string[] markupTokenized = TokenizeMarkup(markup);

            bool collect = false;
            bool templateRunning = false;
            var sbCollected = new StringBuilder();
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
                    collect = true;
                    templateRunning = true;
                    continue;
                }


                if (token == "}}" && indent == 0)
                {
                    collect = false;
                    if(templateRunning)
                        break;
                }

                if (collect)
                    sbCollected.Append(token);

                previousToken = token;
            }

            return sbCollected.ToString();
        }

        //public static List<Template> GetAllMatchingTemplates(string markup, List<string> templateNames)
        //{
        //    string[] markupTokenized = TokenizeMarkup(markup);

        //    bool collect = false;
        //    var sbCollected = new StringBuilder();
        //    var parsedTemplates = new List<string>();
        //    int indent = 0;
        //    var previousToken = "";
        //    foreach (var token in markupTokenized)
        //    {
        //        if (token == "{{")
        //            indent++;

        //        if (token == "}}")
        //            indent--;

        //        if (templateNames.Any(templateName => previousToken + token == "{{" + templateName))
        //        {
        //            collect = true;
        //        }

        //        if (collect && token == "}}" && indent == 0)
        //        {
        //            collect = false;
        //            parsedTemplates.Add(sbCollected.ToString());
        //            sbCollected = new StringBuilder();
        //        }

        //        if (collect)
        //            sbCollected.Append(token);
                
        //        previousToken = token;
        //    }

        //    return parsedTemplates;
        //}

        public static List<Template> GetAllMatchingTemplates(string markup, List<string> templateNames)
        {
            string[] markupTokenized = TokenizeMarkup(markup);

            bool collect = false;
            var languageToken = "";
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
                    languageToken = token;
                    collect = true;
                    continue;
                }

                if (collect && token == "}}" && indent == 0)
                {
                    collect = false;
                    parsedTemplates.Add(new Template(sbCollected.ToString(), languageToken));
                    sbCollected = new StringBuilder();
                }

                if (collect)
                    sbCollected.Append(token);

                previousToken = token;
            }

            return parsedTemplates;
        }

        public static List<Template> GetDescriptionInAllAvailableLanguages(string dscrTemplate)
        {
            return GetAllMatchingTemplates(dscrTemplate, WikiLanguage.GetAllLanguages().Select(l => l.LanguageToken).ToList());
        }

        //public static List<DescriptionInLanguage> GetDescriptionInAllAvailableLanguages(string dscrTemplate)
        //{
        //    string[] markupTokenized = TokenizeMarkup(dscrTemplate);

        //    bool collect = false;
        //    var languageToken = "";
        //    var sbCollected = new StringBuilder();
        //    var parsedDescriptions = new List<DescriptionInLanguage>();
        //    int indent = 0;
        //    var previousToken = "";
        //    foreach (var token in markupTokenized)
        //    {
        //        if (token == "{{")
        //            indent++;

        //        if (token == "}}")
        //            indent--;

        //        if (WikiLanguage.GetAllLanguages().Select(l => l.LanguageToken).Any(templateName => previousToken + token == "{{" + templateName))
        //        {
        //            languageToken = token;
        //            collect = true;
        //            continue;
        //        }

        //        if (collect && token == "}}" && indent == 0)
        //        {
        //            collect = false;
        //            parsedDescriptions.Add(new DescriptionInLanguage(languageToken, sbCollected.ToString()));
        //            sbCollected = new StringBuilder();
        //        }

        //        if (collect)
        //            sbCollected.Append(token);

        //        previousToken = token;
        //    }

        //    return parsedDescriptions;
        //}

        public static string[] TokenizeMarkup(string markup)
        {
            return String.IsNullOrEmpty(markup) ? new string[] {}: (Regex.Split(markup, "({{|}}|\\r|\\n|\\|)"));
        } 
    }
}

public class DescriptionInLanguage
{
    public string LanguageToken;
    public string DescriptionString;

    public DescriptionInLanguage(string languageToken, string descriptionString)
    {
        LanguageToken = languageToken;
        DescriptionString = descriptionString;
    }
}



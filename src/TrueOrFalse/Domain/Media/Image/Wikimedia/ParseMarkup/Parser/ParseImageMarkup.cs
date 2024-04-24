using System.Text.RegularExpressions;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseImageMarkup
    {
        public static ParseImageMarkupResult Run(string markup)
        {
            var result = new ParseImageMarkupResult
            {
                AllRegisteredLicenses =
                    LicenseImage.ToLicenseIdList(LicenseParser.ParseAllRegisteredLicenses(markup))
            };

            var templateFound = false;
            foreach (var infoBoxTemplate in InfoBoxTemplate.GetAllInfoBoxTemplates())
            {
                if (ParseTemplate.GetTemplateByName(markup, infoBoxTemplate.TemplateName).IsSet)
                {
                    result.Template =
                        ParseTemplate.GetTemplateByName(markup, infoBoxTemplate.TemplateName);
                    result.InfoBoxTemplate = infoBoxTemplate;
                    templateFound = true;
                    break;
                }
            }

            if (templateFound)
            {
                ParseDescription.SetDescription_FromTemplate(result);
                ParseAuthor.SetAuthor_FromTemplate(result);

                return result;
            }

            var imageParsingNotifications = new ImageParsingNotifications();
            imageParsingNotifications.InfoTemplate.Add(new Notification
            {
                Name = "No information template found",
                NotificationText =
                    "Autor und Beschreibung konnten nicht automatisch geparsed werden: " +
                    "Es wurde kein Information template gefunden. " +
                    "Bitte Template ergänzen (Klasse InfoBoxTemplate) und/oder Angaben manuell übernehmen.",
            });
            result.Notifications = imageParsingNotifications.ToJson();
            return result;
        }

        public static bool MarkupSyntaxContained(string text)
        {
            return Regex.IsMatch(text, "[{}\\[\\]]"); //Check for "{", "}", "[" or "]"
        }

        public static List<Template> GetDescriptionInAllAvailableLanguages(string dscrTemplate)
        {
            return ParseTemplate.GetAllMatchingTemplates(dscrTemplate,
                WikiLanguage.GetAllLanguages().Select(l => l.LanguageToken).ToList());
        }
    }
}
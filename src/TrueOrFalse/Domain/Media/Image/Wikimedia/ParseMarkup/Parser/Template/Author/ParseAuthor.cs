using System.Text.RegularExpressions;
using static System.String;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseAuthor
    {
        public static void SetAuthor_FromTemplate(ParseImageMarkupResult result)
        {
            var paramAuthor = result.Template.ParamByKey(result.InfoBoxTemplate.AuthorParameterName);
            var paramAuthorUrl = result.Template.ParamByKey(result.InfoBoxTemplate.AuthorParameterNameUrl);

            //$temp: Cases left to match:

            //Mehrteilig:
            //http://commons.wikimedia.org/wiki/File:13-01-15-leipzig-hauptbahnhof-by-RalfR-33.jpg
            //|Author=[[User:Ralf Roletschek|Ralf Roletschek]] ([[User talk:Ralf Roletschek|<span class="signature-talk">talk</span>]]) - [http://www.fahrradmonteur.de Infos �ber Fahrr�der auf fahrradmonteur.de]
            //Gerendert auf Wikimedia: Ralf Roletschek (talk) - Infos �ber Fahrr�der auf fahrradmonteur.de (mit Links auf User, talk und link)
            //Use this file > attribution: By Ralf Roletschek (talk) - Infos �ber Fahrr�der auf fahrradmonteur.de (Own work) [GFDL (http://www.gnu.org/copyleft/fdl.html) or CC-BY-SA-3.0-2.5-2.0-1.0 (http://creativecommons.org/licenses/by-sa/3.0)], via Wikimedia Commons

            //spezielle Attribution templates (nicht am Author erkennbar, nur unter Licenses)
            //...
            //|Author={{User:Noaa/Author}}
            //...
            //== {{int:license}} ==
            //{{self|User:Noaa/AttributionTemplate}}
            //Template can be found here: http://commons.wikimedia.org/wiki/User:Noaa/AttributionTemplate

            //Link unter Autor sollte vielleicht nicht einfach verschwinden
            //https://commons.wikimedia.org/wiki/File:Cavalryatbalaklava2.jpg
            //|Author=[[:en:William Simpson (artist)|William Simpson]]<br/>Published by Paul & Dominic Colnaghi & Co.

            //Erweitert mit Text
            //http://commons.wikimedia.org/wiki/File:Friedhof_G%C3%BCstebieser_Loose_15.JPG
            //|Author=Picture taken by [[User:Marcus Cyron|Marcus Cyron]]

            //$temp:
            //Message setzen, wenn paramAuthor == null

            var imageParsingNotifications = ImageParsingNotifications.FromJson(result.Notifications);

            if (paramAuthor == null)
            {
                imageParsingNotifications.Author.Add(new Notification
                {
                    Name = "No author parameter found",
                    NotificationText = "Es konnte kein Parameter f�r den Autor gefunden werden."
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            if (IsNullOrEmpty(paramAuthor.Value))
            {
                imageParsingNotifications.Author.Add(new Notification
                {
                    Name = "Author parameter empty",
                    NotificationText = "Der Parameter f�r den Autor ist leer."
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            var authorText = Markup2Html.TransformAll(paramAuthor.Value);

            //Handle templates of type "{{User:XRay/Templates/Author}}" (user custom templates), match "User:XRay/Templates/Author"
            //http://commons.wikimedia.org/wiki/File:Unho%C5%A1%C5%A5,_hlavn%C3%AD_t%C5%99%C3%ADda.JPG
            //|Author={{User:Aktron/Author2}}
            //Link to template: http://commons.wikimedia.org/wiki/User:Aktron/Author2
            var regexMatch_UserAttributionTemplate = Regex.Match(authorText, "{{(User:\\w*/.*)}}");
            if (regexMatch_UserAttributionTemplate.Success)
            {
                imageParsingNotifications.Author.Add(new Notification()
                {
                    Name = "Custom wiki user template",
                    NotificationText = Format(
                        "Bitte aus Template \"{0}\" gerenderten Text manuell als Autor von der Bilddetailsseite oder unter <a href=\"{1}\">{1}</a> �bernehmen.",
                        regexMatch_UserAttributionTemplate.Groups[0],
                        "http://commons.wikimedia.org/wiki/" + regexMatch_UserAttributionTemplate.Groups[1])
                });
                    
                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            if (ParseImageMarkup.MarkupSyntaxContained(authorText))
            {
                imageParsingNotifications.Author.Add(new Notification()
                {
                    Name = "Manual entry for author required",
                    NotificationText =
                        Format(
                            "Das Markup f�r den Autor konnte nicht (vollst�ndig) automatisch geparsed werden (es ergab sich: \"{0}\"). Bitte Angaben f�r den Autor manuell �bernehmen.",
                            authorText)
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            if (!IsNullOrEmpty(paramAuthorUrl?.Value))
            {
                result.AuthorName_Raw = paramAuthorUrl.Value + " " + paramAuthor.Value;
                result.AuthorName = $"<a href='{paramAuthorUrl.Value}'>{paramAuthor.Value}</a>";
                return;
            }

            result.AuthorName_Raw = paramAuthor.Value;
            result.AuthorName = authorText;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Impl;
using SolrNet.Mapping.Validation;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseImageMarkup
    {
        public static ParseImageMarkupResult Run(string markup)
        {
            var result = new ParseImageMarkupResult
            {
                AllRegisteredLicenses = LicenseImage.ToLicenseIdList(LicenseParser.ParseAllRegisteredLicenses(markup))
            };

            var templateFound = false;
            foreach (var infoBoxTemplate in InfoBoxTemplate.GetAllInfoBoxTemplates())
            {
                if (ParseTemplate.GetTemplateByName(markup, infoBoxTemplate.TemplateName).IsSet)
                {
                    result.InfoTemplate = ParseTemplate.GetTemplateByName(markup, infoBoxTemplate.TemplateName);
                    result.InfoBoxTemplate = infoBoxTemplate;
                    templateFound = true;
                    break;
                }
            }

            if (templateFound) {
                Care_about_description_and_author(result);

                return result;
            }

            var imageParsingNotifications = new ImageParsingNotifications();
            imageParsingNotifications.InfoTemplate.Add(new Notification()
            {
                Name = "No information template found",
                NotificationText = "Autor und Beschreibung konnten nicht automatisch geparsed werden: " +
                                   "Es wurde kein Information template gefunden. " +
                                   "Bitte Template ergänzen (Klasse InfoBoxTemplate) und/oder Angaben manuell übernehmen.",
            });
            result.Notifications = imageParsingNotifications.ToJson();
            return result;
        }


        private static void Care_about_description_and_author(ParseImageMarkupResult result)
        {
            var paramDesc = result.InfoTemplate.ParamByKey(result.InfoBoxTemplate.DescriptionParamaterName);
            SetDescription(result, paramDesc);

            var paramAuthor = result.InfoTemplate.ParamByKey(result.InfoBoxTemplate.AuthorParameterName);
            SetAuthor(result, paramAuthor);
        }

        private static void SetDescription(ParseImageMarkupResult result, Parameter descrParameter)
        {
            var imageParsingNotifications = ImageParsingNotifications.FromJson(result.Notifications);

            if (descrParameter == null)
            {
                imageParsingNotifications.Description.Add(new Notification()
                {
                    Name = "No description parameter found",
                    NotificationText = "Es konnte kein Parameter für die Beschreibung gefunden werden."
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            if (String.IsNullOrEmpty(descrParameter.Value))
            {
                imageParsingNotifications.Description.Add(new Notification()
                {
                    Name = "Description parameter empty",
                    NotificationText = "Der Parameter für die Beschreibung ist leer."
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }
            
            var preferredLanguages = new List<string>
            {
                //Markup is parsed for description in the following languages (ordered by priority)
                "de", "en", "fr", "es", "ca", "ru", "hu"
            };

            var i = 0;
            var selectedDescrValue = "";

            //Parse for "multilingual description"/"mld"
            var mldSection = ParseTemplate.GetTemplateByName(descrParameter.Value, "Multilingual description").IsSet ?
                ParseTemplate.GetTemplateByName(descrParameter.Value, "Multilingual description") :
                ParseTemplate.GetTemplateByName(descrParameter.Value, "mld");

            while (i < preferredLanguages.Count()) {

                //Check for description in preferred languages (ordered by priority) in "multilingual description"/"mld"
                if (mldSection.IsSet && mldSection.Parameters.Any(x => x.Key == preferredLanguages[i]))
                {
                    if (!String.IsNullOrEmpty(mldSection.ParamByKey(preferredLanguages[i]).Value))
                    {
                        selectedDescrValue = mldSection.ParamByKey(preferredLanguages[i]).Value;
                        break;
                    }
                }

                //Check for preferred languages in seperate description templates
                var langSection = ParseTemplate.GetTemplateByName(descrParameter.Value, preferredLanguages[i]);
                if(langSection.IsSet)
                {
                    if (langSection.Parameters.Any())
                    {
                        if (!String.IsNullOrEmpty(langSection.Parameters.First().Value))
                        {
                            selectedDescrValue = langSection.Parameters.First().Value;
                            break;
                        }
                    }
                }
                i++;
            }

            //If no description in preferred languages is found, search for descriptions in other languages and take the first of them (not very useful since languages are orderer alphabetically)
            if (String.IsNullOrEmpty(selectedDescrValue))
            {
                //Search in "multilingual description"/"mld" parameters
                if (mldSection.Parameters.Any(x => !String.IsNullOrEmpty(x.Value)))
                {
                    selectedDescrValue = mldSection.Parameters.Select(x => x.Value).First(x => !String.IsNullOrEmpty(x));
                }
                //Search in seperate description templates
                else if (GetDescriptionInAllAvailableLanguages(descrParameter.Value).Any(x => x.IsSet))
                {
                    selectedDescrValue = GetDescriptionInAllAvailableLanguages(descrParameter.Value).First(x => x.IsSet).Raw;
                }
            }
            //If suitable mld paramater or description language template has been found
            if (!String.IsNullOrEmpty(selectedDescrValue)) {
                result.Description_Raw = selectedDescrValue;
                var selectedDescrValueTransformed = Markup2Html.TransformAll(selectedDescrValue);

                //If transformed description doesn't contain any templates or markup
                if (!MarkupSyntaxContained(selectedDescrValueTransformed))
                {
                    result.Description = selectedDescrValueTransformed;
                }
                else
                {
                    imageParsingNotifications.Description.Add(new Notification()
                    {
                        Name = "Manual entry for description required",
                        NotificationText = String.Format("Das Markup für die Beschreibung konnte nicht (vollständig) automatisch geparsed werden (es ergab sich: \"{0}\"). Bitte Beschreibung manuell übernehmen.",
                                                            selectedDescrValueTransformed)
                    });
                }
            }
            else
            {
                imageParsingNotifications.Description.Add(new Notification()
                {
                    Name = "Manual entry for description required",
                    NotificationText = String.Format("Es konnte keine Beschreibung in einer zugelassenen Sprache automatisch geparsed werden (Beschreibungstext: \"{0}\"). Bitte falls möglich Beschreibung manuell übernehmen.", 
                                                        Markup2Html.TransformAll(descrParameter.Value))
                });
            }

            result.Notifications = imageParsingNotifications.ToJson();

        }

        private static void SetAuthor(ParseImageMarkupResult result, Parameter paramAuthor)
        {
            //$temp: Cases left to match:

            //Mehrteilig:
            //http://commons.wikimedia.org/wiki/File:13-01-15-leipzig-hauptbahnhof-by-RalfR-33.jpg
            //|Author=[[User:Ralf Roletschek|Ralf Roletschek]] ([[User talk:Ralf Roletschek|<span class="signature-talk">talk</span>]]) - [http://www.fahrradmonteur.de Infos über Fahrräder auf fahrradmonteur.de]
            //Gerendert auf Wikimedia: Ralf Roletschek (talk) - Infos über Fahrräder auf fahrradmonteur.de (mit Links auf User, talk und link)
            //Use this file > attribution: By Ralf Roletschek (talk) - Infos über Fahrräder auf fahrradmonteur.de (Own work) [GFDL (http://www.gnu.org/copyleft/fdl.html) or CC-BY-SA-3.0-2.5-2.0-1.0 (http://creativecommons.org/licenses/by-sa/3.0)], via Wikimedia Commons

            //spezielle Attribution templates (nicht am Author erkennbar, nur unter Licenses)
            //...
            //|Author={{User:Noaa/Author}}
            //...
            //== {{int:license}} ==
            //{{self|User:Noaa/AttributionTemplate}}
            //Template can be found here: http://commons.wikimedia.org/wiki/User:Noaa/AttributionTemplate

//          Link unter Autor sollte vielleicht nicht einfach verschwinden
            //https://commons.wikimedia.org/wiki/File:Cavalryatbalaklava2.jpg
            //|Author=[[:en:William Simpson (artist)|William Simpson]]<br/>Published by Paul & Dominic Colnaghi & Co.

            //Erweitert mit Text
            //http://commons.wikimedia.org/wiki/File:Friedhof_G%C3%BCstebieser_Loose_15.JPG
            //|Author=Picture taken by [[User:Marcus Cyron|Marcus Cyron]]

            //$temp:
            //Message setzen, wenn paramAuthor == null

            var imageParsingNotifications = ImageParsingNotifications.FromJson(result.Notifications);

            if (paramAuthor == null){
                imageParsingNotifications.Author.Add(new Notification()
                {
                    Name = "No author parameter found",
                    NotificationText = "Es konnte kein Parameter für den Autor gefunden werden."
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            if (String.IsNullOrEmpty(paramAuthor.Value))
            {
                imageParsingNotifications.Author.Add(new Notification()
                {
                    Name = "Author parameter empty",
                    NotificationText = "Der Parameter für den Autor ist leer."
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
                    NotificationText = String.Format(
                        "Bitte aus Template \"{0}\" gerenderten Text manuell als Autor von der Bilddetailsseite oder unter <a href=\"{1}\">{1}</a> übernehmen.",
                        regexMatch_UserAttributionTemplate.Groups[0],
                        "http://commons.wikimedia.org/wiki/" + regexMatch_UserAttributionTemplate.Groups[1])
                });
                    
                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }
            if (MarkupSyntaxContained(authorText))
            {
                imageParsingNotifications.Author.Add(new Notification()
                {
                    Name = "Manual entry for author required",
                    NotificationText =
                        String.Format(
                            "Das Markup für den Autor konnte nicht (vollständig) automatisch geparsed werden (es ergab sich: \"{0}\"). Bitte Angaben für den Autor manuell übernehmen.",
                            authorText)
                });

                result.Notifications = imageParsingNotifications.ToJson();
                return;
            }

            result.AuthorName_Raw = paramAuthor.Value;
            result.AuthorName = authorText;
        }

        public static bool MarkupSyntaxContained(string text)
        {
            return Regex.IsMatch(text, "[{}\\[\\]]"); //Check for "{", "}", "[" or "]"
        }

        public static List<Template> GetDescriptionInAllAvailableLanguages(string dscrTemplate)
        {
            return ParseTemplate.GetAllMatchingTemplates(dscrTemplate, WikiLanguage.GetAllLanguages().Select(l => l.LanguageToken).ToList());
        }
    }
}

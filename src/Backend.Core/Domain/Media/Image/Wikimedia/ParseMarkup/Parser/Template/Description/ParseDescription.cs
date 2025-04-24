using static System.String;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseDescription
    {
        public static void SetDescription_FromTemplate(ParseImageMarkupResult result)
        {
            var descriptionParameter =
                result.Template.ParamByKey(result.InfoBoxTemplate.DescriptionParamaterName);

            var imageParsingNotifications =
                ImageParsingNotifications.FromJson(result.Notifications);

            if (descriptionParameter == null)
            {
                AddNotification_DescriptionNotFound(result, imageParsingNotifications);
                return;
            }

            if (IsNullOrEmpty(descriptionParameter.Value))
            {
                AddNotification_AddDescriptionEmpty(result, imageParsingNotifications);
                return;
            }

            var multiLanguageTemplateNames = new[] { "Multilingual description", "title" };
            //Parse for "multilingual description"/"mld"
            var multilingualDescription = ParseTemplate
                .GetTemplateByName(descriptionParameter.Value, multiLanguageTemplateNames).IsSet
                ? ParseTemplate.GetTemplateByName(descriptionParameter.Value,
                    multiLanguageTemplateNames)
                : ParseTemplate.GetTemplateByName(descriptionParameter.Value, "mld");

            var description =
                GetDescription_choose_beste_language(descriptionParameter, multilingualDescription);

            //If suitable mld paramater or description language template has been found
            if (!IsNullOrEmpty(description))
            {
                result.Description_Raw = description;
                var selectedDescrValueTransformed = Markup2Html.TransformAll(description);

                //If transformed description doesn't contain any templates or markup
                if (!ParseImageMarkup.MarkupSyntaxContained(selectedDescrValueTransformed))
                {
                    result.Description = selectedDescrValueTransformed;
                }
                else
                {
                    imageParsingNotifications.Description.Add(new Notification
                    {
                        Name = "Manual entry for description required",
                        NotificationText = Format(
                            "Das Markup für die Beschreibung konnte nicht (vollständig) automatisch geparsed werden (es ergab sich: \"{0}\"). Bitte Beschreibung manuell übernehmen.",
                            selectedDescrValueTransformed)
                    });
                }
            }
            else //multilanguage description not found
            {
                if (!IsNullOrEmpty(descriptionParameter.Value) &&
                    !descriptionParameter.Value.StartsWith("[["))
                {
                    result.Description = Markup2Html.TransformAll(descriptionParameter.Value);
                }
                else
                {
                    imageParsingNotifications.Description.Add(new Notification
                    {
                        Name = "Manual entry for description required",
                        NotificationText = Format(
                            "Es konnte keine Beschreibung in einer zugelassenen Sprache automatisch geparsed werden (Beschreibungstext: \"{0}\"). Bitte falls möglich Beschreibung manuell übernehmen.",
                            Markup2Html.TransformAll(descriptionParameter.Value))
                    });
                }
            }

            result.Notifications = imageParsingNotifications.ToJson();
        }

        private static string GetDescription_choose_beste_language(
            Parameter descrParameter,
            Template multilingualDescriptionTemplate)
        {
            var preferredLanguages = new List<string>
            {
                //Markup is parsed for description in the following languages (ordered by priority)
                "de", "en", "fr", "es", "ca", "ru", "hu"
            };

            var i = 0;
            var selectedDescrValue = "";

            while (i < preferredLanguages.Count)
            {
                //Check for description in preferred languages (ordered by priority) in "multilingual description"/"mld"
                if (multilingualDescriptionTemplate.IsSet &&
                    multilingualDescriptionTemplate.Parameters.Any(x =>
                        x.Key == preferredLanguages[i]))
                {
                    if (!IsNullOrEmpty(multilingualDescriptionTemplate
                            .ParamByKey(preferredLanguages[i]).Value))
                    {
                        selectedDescrValue = multilingualDescriptionTemplate
                            .ParamByKey(preferredLanguages[i]).Value;
                        break;
                    }
                }

                //Check for preferred languages in seperate description templates
                var langSection =
                    ParseTemplate.GetTemplateByName(descrParameter.Value, preferredLanguages[i]);
                if (langSection.IsSet)
                {
                    if (langSection.Parameters.Any())
                    {
                        if (!IsNullOrEmpty(langSection.Parameters.First().Value))
                        {
                            selectedDescrValue = langSection.Parameters.First().Value;
                            break;
                        }
                    }
                }

                i++;
            }

            //If no description in preferred languages is found, search for descriptions in other languages 
            //and take the first of them (not very useful since languages are orderer alphabetically)
            if (IsNullOrEmpty(selectedDescrValue))
            {
                //Search in "multilingual description"/"mld" parameters
                if (multilingualDescriptionTemplate.Parameters.Any(x => !IsNullOrEmpty(x.Value)))
                {
                    selectedDescrValue = multilingualDescriptionTemplate.Parameters
                        .Select(x => x.Value).First(x => !IsNullOrEmpty(x));
                }
                //Search in seperate description templates
                else if (ParseImageMarkup
                         .GetDescriptionInAllAvailableLanguages(descrParameter.Value)
                         .Any(x => x.IsSet))
                {
                    selectedDescrValue = ParseImageMarkup
                        .GetDescriptionInAllAvailableLanguages(descrParameter.Value)
                        .First(x => x.IsSet).Raw;
                }
            }

            return selectedDescrValue;
        }

        private static void AddNotification_AddDescriptionEmpty(
            ParseImageMarkupResult result,
            ImageParsingNotifications imageParsingNotifications)
        {
            imageParsingNotifications.Description.Add(new Notification()
            {
                Name = "Description parameter empty",
                NotificationText = "Der Parameter für die Beschreibung ist leer."
            });

            result.Notifications = imageParsingNotifications.ToJson();
        }

        private static void AddNotification_DescriptionNotFound(
            ParseImageMarkupResult result,
            ImageParsingNotifications imageParsingNotifications)
        {
            imageParsingNotifications.Description.Add(new Notification
            {
                Name = "No description parameter found",
                NotificationText = "Es konnte kein Parameter für die Beschreibung gefunden werden."
            });

            result.Notifications = imageParsingNotifications.ToJson();
        }
    }
}
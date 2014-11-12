using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using NHibernate.Impl;
using SolrNet.Mapping.Validation;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseImageMarkup
    {
        
        public static ParseImageMarkupResult Run(string markup)
        {
            var result = new ParseImageMarkupResult();
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
            };

            if (templateFound) {
                Care_about_description_and_author(result);
                Care_about_license_template(markup, result);
            }

            return result;
        }

        private static void Care_about_license_template(string markup, ParseImageMarkupResult result)
        {
            //http://en.wikipedia.org/wiki/Template:Self
            var selfTemplate = ParseTemplate.GetTemplateByName(markup, "self");
            if (selfTemplate.IsSet)
            {
                var allLicenseTemplates = selfTemplate.Parameters.Where(x => !x.HasKey).ToList();

                Func<Parameter, string, bool> fnPredicate = (x, startsWith) => x.Value.ToLower().StartsWith(startsWith);
                if (allLicenseTemplates.Any(x => fnPredicate(x, "pd")))
                {
                    result.LicenseIsPublicDomain = true;
                    result.LicenseTemplateString = allLicenseTemplates.First(x => fnPredicate(x, "pd")).Value;
                }
                else if (allLicenseTemplates.Any(x => fnPredicate(x, "gfdl")))
                {
                    result.LicenseIsGFDL = true;
                    result.LicenseTemplateString = allLicenseTemplates.First(x => fnPredicate(x, "gfdl")).Value;
                }
                else if (allLicenseTemplates.Any(x => fnPredicate(x, "cc-")))
                {
                    result.LicenseIsCreativeCommons = true;
                    result.LicenseTemplateString = allLicenseTemplates.First(x => fnPredicate(x, "cc-")).Value;
                }

            }            
        }

        private static void Care_about_description_and_author(ParseImageMarkupResult result)
        {
            var paramDesc = result.InfoTemplate.ParamByKey(result.InfoBoxTemplate.DescriptionParamaterName);
            if (paramDesc != null)
                SetDescription(result, paramDesc);

            var paramAuthor = result.InfoTemplate.ParamByKey(result.InfoBoxTemplate.AuthorParameterName);
            if (paramAuthor != null)
            {
                result.AuthorName_Raw = paramAuthor.Value;
                result.AuthorName = Markup2Html.Run(paramAuthor.Value);
            }
        }

        private static void SetDescription(ParseImageMarkupResult result, Parameter descrParameter)
        {
            var preferredLanguages = new List<string>
            {
                //Markup is parsed for description in the following languages (ordered by priority)
                "de", "en", "fr", "es", "ca", "ru", "hu"
            };

            var i = 0;
            var paramValue = "";
            var mldSection = ParseTemplate.GetTemplateByName(descrParameter.Value, "Multilingual description").IsSet ?
                ParseTemplate.GetTemplateByName(descrParameter.Value, "Multilingual description") :
                ParseTemplate.GetTemplateByName(descrParameter.Value, "mld");

            while (i < preferredLanguages.Count()) {

                //Parse for "multilingual description"/"mld"


                //Check for description in preferred languages (ordered by priority) in "multilingual description"/"mld"
                if (mldSection.Parameters.Any(x => x.Key == preferredLanguages[i]))
                {
                    paramValue = mldSection.ParamByKey(preferredLanguages[i]).Value;
                    break;
                }

                //Check for preferred languages in seperate description templates
                var langSection = ParseTemplate.GetTemplateByName(descrParameter.Value, preferredLanguages[i]);
                if(langSection.IsSet)
                {
                    if (langSection.Parameters.Any())
                    {
                        paramValue = langSection.Parameters.First().Value;
                        break;
                    }
                }

                i++;
            }

            //If no description in preferred languages is found, search for descriptions in other languages and take the first of them
            if (String.IsNullOrEmpty(paramValue))
            {
                //Search in "multilingual description"/"mld"
                if (mldSection.Parameters.Any())
                {
                    paramValue = mldSection.Parameters.First().Value;
                }
                //Search in seperate description templates
                else if (GetDescriptionInAllAvailableLanguages(descrParameter.Value).Any())
                {
                    paramValue = GetDescriptionInAllAvailableLanguages(descrParameter.Value).Select(d => d.Raw).First();
                }

            }

            if (!String.IsNullOrEmpty(paramValue)) { 
                result.Description_Raw = paramValue;
                result.Description = Markup2Html.Run(paramValue);
            } 
            else if (!String.IsNullOrEmpty(descrParameter.Value)
                       && !descrParameter.Value.Contains("{{")
                       && !descrParameter.Value.Contains("}}"))
            {
                //If description doesn't contain any templates, just plain text
                result.Description_Raw = descrParameter.Value;
                result.Description = Markup2Html.Run(descrParameter.Value);
            }
        }

        public static List<Template> GetDescriptionInAllAvailableLanguages(string dscrTemplate)
        {
            return ParseTemplate.GetAllMatchingTemplates(dscrTemplate, WikiLanguage.GetAllLanguages().Select(l => l.LanguageToken).ToList());
        }
    }
}

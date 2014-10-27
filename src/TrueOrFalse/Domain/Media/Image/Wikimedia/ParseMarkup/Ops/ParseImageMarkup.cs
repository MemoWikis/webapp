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
            result.InfoTemplate = new Template(ParseTemplate.Run(markup, "Information"));

            Care_about_description_and_author(result);
            Care_about_license_template(markup, result);

            return result;
        }

        private static void Care_about_license_template(string markup, ParseImageMarkupResult result)
        {
            //http://en.wikipedia.org/wiki/Template:Self
            var selfTemplate = new Template(ParseTemplate.Run(markup, "self"));
            if (selfTemplate.IsSet)
            {
                var allLicenseTemplates = selfTemplate.Parameters.Where(x => !x.HasKey).ToList();

                Func<Parameter, string, bool> fnPredicate = (x, startsWith) => x.Value.ToLower().StartsWith(startsWith);
                if (allLicenseTemplates.Any(x => fnPredicate(x, "pd")))
                {
                    result.LicenseIsPublicDomain = true;
                    result.LicenceTemplateString = allLicenseTemplates.First(x => fnPredicate(x, "pd")).Value;
                }
                else if (allLicenseTemplates.Any(x => fnPredicate(x, "gfdl")))
                {
                    result.LicenseIsGFDL = true;
                    result.LicenceTemplateString = allLicenseTemplates.First(x => fnPredicate(x, "gfdl")).Value;
                }
                else if (allLicenseTemplates.Any(x => fnPredicate(x, "cc-")))
                {
                    result.LicenceIsCreativeCommons = true;
                    result.LicenceTemplateString = allLicenseTemplates.First(x => fnPredicate(x, "cc-")).Value;
                }

            }            
        }

        private static void Care_about_description_and_author(ParseImageMarkupResult result)
        {
            var paramDesc = result.InfoTemplate.ParamByKey("Description");
            if (paramDesc != null)
                SetDescription(result, paramDesc);
            else
            {
                result.InfoTemplate.ParamByKey("BArch-image");

            }

            var paramAuthor = result.InfoTemplate.ParamByKey("Author");
            if (paramAuthor != null)
            {
                result.AuthorName_Raw = paramAuthor.Value;
                result.AuthorName = Markup2Html.Run(paramAuthor.Value);
            }
        }

        private static void SetDescription(ParseImageMarkupResult result, Parameter descnParameter)
        {
            var mldSection = new Template(ParseTemplate.Run(descnParameter.Value, "mld"));
            if (mldSection.Parameters.Any(x => x.Key == "de"))
            {
                var deParamValue = mldSection.ParamByKey("de").Value;
                result.DescriptionDE_Raw = deParamValue;
                result.Description = Markup2Html.Run(deParamValue);
                return;
            }

            var deSection = new Template(ParseTemplate.Run(descnParameter.Value, "de"));
            if(deSection.IsSet)
            {
                if (deSection.Parameters.Any())
                {
                    var deParamValue = deSection.Parameters.First().Value;
                    result.DescriptionDE_Raw = deParamValue;
                    result.Description = Markup2Html.Run(deParamValue);
                }
                return;
            }
        }

    }
}

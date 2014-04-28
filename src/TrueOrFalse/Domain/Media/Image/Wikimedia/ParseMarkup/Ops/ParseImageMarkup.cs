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
            Care_about_licence_template(markup, result);

            return result;
        }

        private static void Care_about_licence_template(string markup, ParseImageMarkupResult result)
        {
            //http://en.wikipedia.org/wiki/Template:Self
            var selfTemplate = new Template(ParseTemplate.Run(markup, "self"));
            if (selfTemplate.IsSet)
            {
                var allLicenceTemplates = selfTemplate.Parameters.Where(x => !x.HasKey).ToList();

                Func<Parameter, string, bool> fnPredicate = (x, startsWith) => x.Value.ToLower().StartsWith(startsWith);
                if (allLicenceTemplates.Any(x => fnPredicate(x, "pd")))
                {
                    result.LicenceIsPublicDomain = true;
                    result.LicenceTemplateString = allLicenceTemplates.First(x => fnPredicate(x, "pd")).Value;
                }
                else if (allLicenceTemplates.Any(x => fnPredicate(x, "gfdl")))
                {
                    result.LicenceIsGFDL = true;
                    result.LicenceTemplateString = allLicenceTemplates.First(x => fnPredicate(x, "gfdl")).Value;
                }
                else if (allLicenceTemplates.Any(x => fnPredicate(x, "cc-")))
                {
                    result.LicenceIsCreativeCommons = true;
                    result.LicenceTemplateString = allLicenceTemplates.First(x => fnPredicate(x, "cc-")).Value;
                }

            }            
        }

        private static void Care_about_description_and_author(ParseImageMarkupResult result)
        {
            var paramDesc = result.InfoTemplate.ParamByKey("Description");
            if (paramDesc != null)
            {
                var mldSection = new Template(ParseTemplate.Run(paramDesc.Value, "mld"));
                if (mldSection.Parameters.Any(x => x.Key == "de"))
                {
                    var deParamValue = mldSection.ParamByKey("de").Value;
                    result.DescriptionDE_Raw = deParamValue;
                    result.Description = Markup2Html.Run(deParamValue);
                }
            }

            var paramAuthor = result.InfoTemplate.ParamByKey("Author");
            if (paramAuthor != null)
            {
                result.AuthorName_Raw = paramAuthor.Value;
                result.AuthorName = Markup2Html.Run(paramAuthor.Value);
            }
        }
    }
}

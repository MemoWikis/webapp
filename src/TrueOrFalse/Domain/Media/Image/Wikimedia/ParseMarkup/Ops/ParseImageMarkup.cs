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
            var result = new ParseImageMarkupResult{
                InfoTemplate = new Template(ParseTemplate.Run(markup, "Information"))
            };

            var paramDesc = result.InfoTemplate.ParamByKey("Description");
            if (paramDesc != null){
                var mldSection = new Template(ParseTemplate.Run(paramDesc.Value, "mld"));
                if (mldSection.Parameters.Any(x => x.Key == "de")){
                    var deParamValue = mldSection.ParamByKey("de").Value;
                    result.DescriptionDE_Raw = deParamValue;
                    result.Description = Markup2Html.Run(deParamValue);
                }       
            }

            var paramAuthor = result.InfoTemplate.ParamByKey("Author");
            if (paramAuthor != null){
                result.AuthorName_Raw = paramAuthor.Value;
                result.AuthorName = Markup2Html.Run(paramAuthor.Value);
            }

            return result;
        }

    }
}

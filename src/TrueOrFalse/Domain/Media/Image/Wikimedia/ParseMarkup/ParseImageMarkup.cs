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

            var paramDescKey = result.InfoTemplate.ParamByKey("Description");
            if (paramDescKey != null)
            {
                var mldSection = new Template(ParseTemplate.Run(paramDescKey.Value, "mld"));
                if(mldSection.Parameters.Any(x => x.Key == "de"))
                    result.DescriptionDE_Raw = mldSection.ParamByKey("de").Value;
            }

            return result;
        }

    }
}

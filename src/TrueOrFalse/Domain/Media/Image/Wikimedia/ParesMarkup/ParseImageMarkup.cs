using System;
using System.Collections.Generic;
using System.Linq;
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
            result.InformationTemplate = new Template { Raw = ParseTemplate.Run(markup, "Information") };
            result.InformationTemplate.Parameters = ParseTemplateParameters.Run(result.InformationTemplate.Raw);

            return result;
        }

    }
}

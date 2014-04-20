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
    public class ParseImagePageMarkup
    {
        public Section InformationSection;

        private readonly string[] _markupTokenized;

        public ParseImagePageMarkup(string markup)
        {
            markup = markup.Replace("\r\n", "");
            _markupTokenized = markup.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            InformationSection = new Section { Text = GetTemplateSection(",", "Information") };
            InformationSection.Parameters = ParseSectionParameters.Run(InformationSection.Text);
        }


        public string GetTemplateSection(string text, string section)
        {
            bool collect = false;
            var sbCollected = new StringBuilder();
            int indent = 0;
            foreach (var token in _markupTokenized)
            {
                if (token.StartsWith("{{"))
                    indent++;

                if (token.EndsWith("}}"))
                    indent--;

                if (token == "{{" + section)
                {
                    collect = true;
                    continue;
                }
                    
                if (token == "}}" && indent == 0)
                    collect = false;

                if (collect)
                    sbCollected.Append(token);
            }

            return sbCollected.ToString();
        }
    }
}

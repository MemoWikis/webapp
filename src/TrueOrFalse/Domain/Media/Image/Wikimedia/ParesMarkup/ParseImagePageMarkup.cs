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
            InformationSection.Parameters = GetParameters(InformationSection.Text);
        }

        enum States{ Detault, ParameterStarted}

        public List<Parameter> GetParameters(string section)
        {
            var textTokens = Regex.Split(section, "(\\||{{|}}|\\[\\[|\\]\\]|=)");

            int level = 0;

            var result = new List<Parameter>();
            var enumerator = textTokens.GetEnumerator();
            var currentParameterTokens = new List<string>();
            var state = States.Detault;
            while (enumerator.MoveNext())
            {
                var token = (string)enumerator.Current;

                //ignore parameters of subtemplates
                if (new[] { "[[", "{{" }.Any(x => x == token))
                    level++;


                if (new[] { "]]", "}}" }.Any(x => x == token))
                    level--;

                //parameter started
                if (token == "|" && level == 0){ 

                    if (currentParameterTokens.Any())
                        result.Add(new Parameter(currentParameterTokens));

                    state = States.ParameterStarted;
                    currentParameterTokens = new List<string>();

                    continue;
                }

                //collect parameter data
                if (state == States.ParameterStarted)
                    currentParameterTokens.Add(token);
            }

            return result;
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

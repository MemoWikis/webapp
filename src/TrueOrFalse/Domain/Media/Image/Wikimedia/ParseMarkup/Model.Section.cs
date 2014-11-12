using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    [DebuggerDisplay("Raw={Raw}")]
    public class Template
    {
        public string TemplateName;
        public string Raw;
        public List<Parameter> Parameters = new List<Parameter>();
        public Parameter SuperordinateParameter;

        public bool IsSet{ get { return !String.IsNullOrEmpty(Raw); } }

        public Template(string text, string templateName)
        {
            TemplateName = templateName;
            Raw = text;
            Parameters = ParseTemplateParameters.Run(text);
        }

        public Parameter ParamByKey(string key)
        {
            return Parameters.ByKey(key);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    [DebuggerDisplay("Text={Text}")]
    public class Template
    {
        public string Raw;
        public List<Parameter> Parameters = new List<Parameter>();
    }
}

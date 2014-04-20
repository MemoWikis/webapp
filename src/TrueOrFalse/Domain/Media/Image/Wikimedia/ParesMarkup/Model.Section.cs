using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    [DebuggerDisplay("Text={Text}")]
    public class Section
    {
        public string Text;
        public List<Parameter> Parameters = new List<Parameter>();
    }
}

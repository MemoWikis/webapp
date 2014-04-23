using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    [DebuggerDisplay("Raw={Raw}")]
    public class Parameter
    {
        public string Key;
        public string Value;

        public string Raw;
        public List<string> Tokens;

        public Parameter(List<string> currentParameterTokens)
        {
            Raw = currentParameterTokens.Aggregate((a,b) => a + ' ' + b);
            Tokens = currentParameterTokens;

            if (Raw.Contains("="))
            {
                bool isKey = true;
                bool isValue = false;
                foreach (var token in Tokens)
                {
                    if (token == "=" && !isValue){
                        isValue = true;
                        isKey = false;
                        continue;
                    }

                    if (isKey)
                        Key += token;

                    if (isValue)
                        Value += token;
                }

                if (Key != null) Key = Key.Trim();
                if (Value != null) Value = Value.Trim();
            }
        }
    }
}
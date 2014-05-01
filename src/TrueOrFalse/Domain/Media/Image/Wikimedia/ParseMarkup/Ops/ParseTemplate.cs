using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseTemplate
    {
        public static string Run(string markup, string templateName)
        {
            string[] markupTokenized = Regex.Split(markup, "({{|}}|\\r|\\n|\\|)"); ;

            bool collect = false;
            var sbCollected = new StringBuilder();
            int indent = 0;
            var previousToken = "";
            foreach (var token in markupTokenized)
            {
                if (token == "{{")
                    indent++;

                if (token == "}}")
                    indent--;

                if (previousToken + token == "{{" + templateName)
                {
                    collect = true;
                    continue;
                }

                if (token == "}}" && indent == 0)
                    collect = false;

                if (collect)
                    sbCollected.Append(token);

                previousToken = token;
            }

            return sbCollected.ToString();
        }
    }
}

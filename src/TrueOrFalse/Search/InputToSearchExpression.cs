using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NHibernate.Util;

namespace TrueOrFalse.Search
{
    public class InputToSearchExpression
    {
        public static string Run(string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
                return "";

            var multiWordTermsResult = Regex.Match(userInput, "\".*\"");
            if (multiWordTermsResult.Captures.Any())
            {
                foreach (Match capture in multiWordTermsResult.Captures)
                    userInput = userInput.Replace(capture.Value, "");
            }

            var userInputParts = userInput.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);
            userInputParts[userInputParts.Count()-1] = userInputParts[userInputParts.Count()-1] + "~";

            var result = "(" + userInputParts.Aggregate((a,b) => a + "~ " + b ) + ")";

            foreach (Match capture in multiWordTermsResult.Captures)
                result = result.Insert(result.Length - 1, " " + capture.Value.Replace("\"",""));

            return result;
        }
    }
}
    
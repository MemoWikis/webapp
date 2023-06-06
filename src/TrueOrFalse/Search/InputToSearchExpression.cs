using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Util;

namespace TrueOrFalse.Search
{
    public class InputToSearchExpression
    {
        public static string Run(string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
                return "";

            if (userInput.Trim() == "")
                return "";

            var multiWordTermsResult = Regex.Match(userInput, "\".*\"");
            if (multiWordTermsResult.Captures.Any())
            {
                foreach (Match capture in multiWordTermsResult.Captures)
                    userInput = userInput.Replace(capture.Value, "");
            }

            //exact search
            if (String.IsNullOrEmpty(userInput))
            {
                var result1 = "()";
                foreach (Match capture in multiWordTermsResult.Captures)
                    result1 = result1.Insert(result1.Length - 1, capture.Value.Replace("\"", ""));

                return result1;
            }

            var userInputParts = userInput.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            userInputParts[userInputParts.Count() - 1] = userInputParts[userInputParts.Count() - 1] + "~";

            var result = "(" + userInputParts.Aggregate((a, b) => a + "~ " + b) + ")";                

            foreach (Match capture in multiWordTermsResult.Captures)
                result = result.Insert(result.Length - 1, " " + capture.Value.Replace("\"",""));

            return result;
        }
    }
}
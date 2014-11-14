using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    public class Markup2Html
    {
        public static string Run(string markup)
        {
            //finds "[[de:pageName|displayName]]" transforms to "displayName"
            markup = Regex.Replace(markup, "\\[\\[.*?\\|(.*?)\\]\\]", match => match.Groups[1].Value);
            //finds "[[displayName]]" transforms to "displayName"
            markup = Regex.Replace(markup, "\\[\\[(.*?)\\]\\]", match => match.Groups[1].Value);
            //finds "[displayName]" transforms to "displayName"
            markup = Regex.Replace(markup, "\\[(.*?)\\]", match => match.Groups[1].Value);
            //finds "''displayName''" transforms to "<i>displayName</i>"
            markup = Regex.Replace(markup, "''(.*?)''", match => "<i>" + match.Groups[1].Value + "</i>");
            //$temp: Add //finds "[http://link.com  diplayname]" transforms to ... ?

            return markup;
        }
    }
}

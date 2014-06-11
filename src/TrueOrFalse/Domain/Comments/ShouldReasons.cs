using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrueOrFalse
{
    public class ShouldReasons
    {
        private static Dictionary<string, string> _shouldReasons = new Dictionary<string, string>();

        static ShouldReasons()
        {
            _shouldReasons.Add("shouldBePrivate", "Die Frage sollte privat sein.");
            _shouldReasons.Add("sourcesAreWrong", "Die Quellen sind nicht korrekt.");
            _shouldReasons.Add("answerNotClear", "Die Antwort ist nicht eindeutig.");
            _shouldReasons.Add("improveOtherReason", "... ein anderer Grund.");
            _shouldReasons.Add("", "");
        }

        public static string ByKey(string key)
        {
            return _shouldReasons[key];
        }

        public static List<string> ByKeys(string keysCsv)
        {
            if(String.IsNullOrEmpty(keysCsv))
                return new List<string>();

            return keysCsv.Split(',').Select(x => x.Trim()).Select(ByKey).ToList();
        }
    }

}

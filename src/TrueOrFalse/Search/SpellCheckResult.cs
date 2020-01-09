using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Impl;
using SolrNet.Impl.ResponseParsers;

namespace TrueOrFalse.Search
{
    [Serializable]
    public class SpellCheckResult
    {
        public string Collation;
        public int Count;
        public List<SpellCheckResultItem> Items = new List<SpellCheckResultItem>();

        public SpellCheckResult(SpellCheckResults spellChecking, string searchTerm)
        {
            Collation = spellChecking.Collation;
            Count = spellChecking.Count;
            foreach (var spellCheck in spellChecking)
                Items.Add(new SpellCheckResultItem(spellCheck, searchTerm));
        }

        public string GetSuggestion()
        {
            if (Items.Any())
            {
                //var words = Items.Select(spellCheckItem => spellCheckItem.Suggestions.First()).ToList();
                //return words.GroupBy(x => x).Select(x => x.Key).Aggregate((a, b) => a + " " + b);

                var spellCheckResult = Items.First();

                if (spellCheckResult.Suggestions.Any())
                    return spellCheckResult.Suggestions.First();
            }
            return null;
        }
    }

    [Serializable]
    public class SpellCheckResultItem
    {
        public string Query;
        public int NumFound;
        public int StartOffset;
        public int EndOffset;
        public IList<string> Suggestions;

        public SpellCheckResultItem(SolrNet.Impl.SpellCheckResult spellCheck, string searchTerm)
        {
            if (searchTerm == null)
                searchTerm = "";

            Query = spellCheck.Query;
            NumFound = spellCheck.NumFound;
            EndOffset = spellCheck.EndOffset;
            StartOffset = spellCheck.StartOffset;

            if (spellCheck.Suggestions == null)
                spellCheck.Suggestions = new List<string>();
            else
                Suggestions = spellCheck.Suggestions
                    .Where(s => s.ToLower().Trim() != searchTerm.ToLower().Trim())
                    .ToList();
        }
    }
}

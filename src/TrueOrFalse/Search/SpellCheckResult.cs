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

        public SpellCheckResult(SpellCheckResults spellChecking)
        {
            Collation = spellChecking.Collation;
            Count = spellChecking.Count;
            foreach (var spellCheck in spellChecking)
                Items.Add(new SpellCheckResultItem(spellCheck));
        }

        public string GetSuggestion()
        {
            if (Items.Any())
            {
                var spellCheckResult = Items.First();
                return spellCheckResult.Suggestions.First();
            }
            return null;
        }
    }

    [Serializable]
    public class SpellCheckResultItem
    {
        public int NumFound;
        public int StartOffset;
        public int EndOffset;
        public ICollection<string> Suggestions;

        public SpellCheckResultItem(SolrNet.Impl.SpellCheckResult spellCheck)
        {
            NumFound = spellCheck.NumFound;
            EndOffset = spellCheck.EndOffset;
            StartOffset = spellCheck.StartOffset;
            Suggestions = spellCheck.Suggestions;
        }
    }
}

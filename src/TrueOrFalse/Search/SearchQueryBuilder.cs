using System;
using System.Text;
using TrueOrFalse.Search;

namespace TrueOrFalse.Search
{
    public class SearchQueryBuilder
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public SearchQueryBuilder Add(string fieldName, string seachTerm, bool startsWith = false)
        {
            if (!String.IsNullOrEmpty(seachTerm))
                _sb.Append(fieldName + ": " + InputToSearchExpression.Run(seachTerm) + " ");

            return this;
        }

        public override string ToString()
        {
            return _sb.ToString().Trim();
        }

    }
}
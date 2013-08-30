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
            if (String.IsNullOrEmpty(seachTerm))
                return this;

            if(startsWith)
                _sb.Append(fieldName + ":(" + seachTerm + "* OR " + seachTerm + "~ )");
            else
                _sb.Append(fieldName + ": " + InputToSearchExpression.Run(seachTerm) + " ");

            return this;
        }

        public override string ToString()
        {
            return _sb.ToString().Trim();
        }

    }
}
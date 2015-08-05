using System;
using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search
{
    public class SearchQueryBuilder
    {
        private string _buildedExpression;

        private readonly IList<string> _orConditions = new List<string>();
        private readonly IList<string> _andConditions = new List<string>();

        public SearchQueryBuilder Add(
            string fieldName, 
            string seachTerm, 
            bool startsWith = false,
            bool exact = false,
            bool phrase = false,
            bool isAndCondition = false)
        {
            if (String.IsNullOrWhiteSpace(seachTerm))
                return this;

            seachTerm = seachTerm.Trim();

            string term;
            if (exact)
            {
                term = fieldName + ":(" + seachTerm + ")";
            }
            else if (phrase && !seachTerm.StartsAndEndsWith("\""))
            {
                term = fieldName + ":(\"" + seachTerm + "\")^10";
            }
            else if (startsWith)
            {
                if (seachTerm.StartsWith("\"") && seachTerm.EndsWith("\""))
                    term = fieldName + ":(" + seachTerm.Insert(seachTerm.Length - 1, "*") + ")^10";
                else
                    term = fieldName + ":(" + seachTerm + "*)^10";
            }
            else
                term = fieldName + ":" + InputToSearchExpression.Run(seachTerm);

            if(isAndCondition)
                _andConditions.Add(term);
            else
                _orConditions.Add(term);

            return this;
        }

        private void BuildExpression()
        {
            _buildedExpression = "*:*";

            if (!_orConditions.Any() && !_andConditions.Any())
                return;

            string stringOrConditions = null;
            if(_orConditions.Any())
                stringOrConditions = _orConditions.Aggregate((a, b) => a + " " + b);

            if (!String.IsNullOrEmpty(stringOrConditions) && _andConditions.Count == 0)
                _buildedExpression = stringOrConditions;
            else if (!String.IsNullOrEmpty(stringOrConditions) && _andConditions.Count != 0)
                _buildedExpression = "(" + stringOrConditions + ")" + " AND " + _andConditions.Aggregate((a,b) => a + " AND " + b);
            else
                _buildedExpression = _andConditions.Aggregate((a, b) => a + " AND " + b);
        }

        public override string ToString()
        {
            if (_buildedExpression == null)
                BuildExpression();

            return _buildedExpression;
        }

    }
}
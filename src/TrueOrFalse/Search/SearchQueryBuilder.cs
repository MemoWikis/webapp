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
        private readonly IList<string> _notConditions = new List<string>();

        public SearchQueryBuilder Add(
            string fieldName, 
            string seachTerm, 
            bool startsWith = false,
            bool exact = false,
            bool phrase = false,
            bool isAndCondition = false, 
            bool isNotCondition = false,
            int boost = -1)
        {
            if (String.IsNullOrWhiteSpace(seachTerm))
                return this;

            seachTerm = seachTerm.Trim();

            string term;
            if (exact)
            {
                term = fieldName + ":(" + seachTerm + ")";
	            if (boost >= 0)
		            term = "^" + boost;
            }
            else if (phrase && !seachTerm.StartsAndEndsWith("\""))
            {
                term = fieldName + ":(\"" + seachTerm + "\")^10";
            }
            else if (startsWith)
            {
	            if (seachTerm.StartsWith("\"") && seachTerm.EndsWith("\""))
		            term = fieldName + ":(" + seachTerm.Insert(seachTerm.Length - 1, "*") + ")";
	            else
		            term = fieldName + ":(" + seachTerm + "*)";

	            if (boost == -1)
		            term += "^10";
                else
                    term += "^" + boost;
            }
            else
            {
                term = fieldName + ":" + InputToSearchExpression.Run(seachTerm);
                if (boost >= 0)
                    term += "^" + boost;
            }
                

            if(isAndCondition)
                _andConditions.Add(term);
            else if (isNotCondition)
                _notConditions.Add(term);
            else
                _orConditions.Add(term);

            return this;
        }

        private void BuildExpression()
        {
            _buildedExpression = "*:*";

            if (!_orConditions.Any() && !_andConditions.Any() && !_notConditions.Any())
                return;

            string stringOrConditions = null;
            if(_orConditions.Any())
                stringOrConditions = "(" + _orConditions.Aggregate((a, b) => a + " " + b) + ")";

            var andConditions = _andConditions.Any() ? _andConditions.Aggregate((a, b) => a + " AND " + b) : "";
            var notConditions = _notConditions.Any() ? _notConditions.Select(a => "-" + a).Aggregate((a, b) => a + " AND " + b) : "";

            _buildedExpression = new List<string> { stringOrConditions, andConditions, notConditions}
                .Where(a => !String.IsNullOrEmpty(a))
                .Aggregate((a, b) => a + " AND " + b);
        }

        public override string ToString()
        {
            if (_buildedExpression == null)
                BuildExpression();

            return _buildedExpression;
        }

    }
}
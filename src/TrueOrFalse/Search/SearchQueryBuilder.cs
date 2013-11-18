using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Search;

namespace TrueOrFalse.Search
{
    public class SearchQueryBuilder
    {
        private string _buildedExpression;

        public IList<string> _orConditions = new List<string>();
        public IList<string> _andConditions = new List<string>();

        public SearchQueryBuilder Add(
            string fieldName, 
            string seachTerm, 
            bool startsWith = false,
            bool exact = false,
            bool isMustHave = false)
        {
            if (String.IsNullOrEmpty(seachTerm))
                return this;

            string term;
            if(exact)
                term = fieldName + ":(" + seachTerm + ")";
            else if(startsWith)
                term = fieldName + ":(" + seachTerm + "* OR " + seachTerm + "~ )";
            else
                term = fieldName + ":" + InputToSearchExpression.Run(seachTerm);

            if(isMustHave)
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
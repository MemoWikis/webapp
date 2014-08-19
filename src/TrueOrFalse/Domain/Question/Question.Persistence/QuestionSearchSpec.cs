using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Mapping;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;
using SpellCheckResult = TrueOrFalse.Search.SpellCheckResult;

namespace TrueOrFalse
{
    [Serializable]
    public class QuestionSearchSpec : SearchSpecificationBase<QuestionFilter, QuestionOrderBy>
    {
        public QuestionSearchSpec(bool ignorePrivates = true)
        {
            Filter.IgnorePrivates = ignorePrivates;
        }

        public string Key;
        public SearchTab SearchTab;
        public SpellCheckResult SpellCheck;

        public QuestionHistoryItem HistoryItem;

        public string GetSuggestion()
        {
            if (SpellCheck == null)
                return "";

            return SpellCheck.GetSuggestion();
        }
    }

    [Serializable]
    public class QuestionFilter : ConditionContainer
    {
        public string SearchTerm;
        public int CreatorId = -1;
        public int ValuatorId = -1;
        public bool IgnorePrivates = true;

        public IList<int> Categories = new List<int>();

        public static string GetCategoryFilterValue(string searchTerm)
        {
            return GetFilter("Kat", searchTerm);
        }

        public static string GetCreatorFilterValue(string searchTerm)
        {
            return GetFilter("Ersteller", searchTerm);
        }

        private static string GetFilter(string key, string searchTerm)
        {
            string filter = null;
            if (searchTerm != null && searchTerm.IndexOf(key + ":\"") != -1)
            {
                var match = Regex.Match(searchTerm, key + ":\"(.*)\"", RegexOptions.IgnoreCase);
                if (match.Success)
                    filter = match.Groups[1].Value;
            }
            return filter;
        }

        public void Clear()
        {
            SearchTerm = "";
            CreatorId = -1;
            ValuatorId = -1;
            IgnorePrivates = true;
            Categories.Clear();

            base.Clear();
        }

        public bool IsOneCategoryFilter()
        {
            return Categories.Count == 1 && SearchTerm == "";
        }
    }

    [Serializable]
    public class QuestionOrderBy : SpecOrderByBase
    {
        public OrderBy OrderByPersonalRelevance;
        public OrderBy OrderByQuality;
        public OrderBy OrderByViews;

        public OrderBy OrderByCreationDate;

        public QuestionOrderBy()
        {
            OrderByPersonalRelevance = new OrderBy("TotalRelevancePersonalAvg", this);
            OrderByQuality = new OrderBy("TotalQualityAvg", this);
            OrderByViews = new OrderBy("TotalViews", this);
            OrderByCreationDate = new OrderBy("DateCreated", this);
        }

        public string ToText()
        {
            if (OrderByPersonalRelevance.IsCurrent())
                return "Gemerkt";

            if (OrderByQuality.IsCurrent())
                return "Qualität";

            if (OrderByViews.IsCurrent())
                return "Gesehen";

            if (OrderByCreationDate.IsCurrent())
                return "Datum erstellt";

            return "";
        }
    }
}
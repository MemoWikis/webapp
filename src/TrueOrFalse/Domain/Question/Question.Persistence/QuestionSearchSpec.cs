using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Seedworks.Lib.Persistence;

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
        public string KeyOverviewPage;
    }

    [Serializable]
    public class QuestionFilter : ConditionContainer
    {
        public string SearchTerm;
        public int CreatorId = -1;
        public int ValuatorId = -1;
        public bool IgnorePrivates = true;
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
                return "Merken";

            if (OrderByQuality.IsCurrent())
                return "Qualität";

            if (OrderByViews.IsCurrent())
                return "Ansichten";

            if (OrderByCreationDate.IsCurrent())
                return "Erstellungsdatum";

            return "";
        }
    }
}
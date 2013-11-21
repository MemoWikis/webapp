using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    public class QuestionSearchSpec : SearchSpecificationBase<QuestionFilter, QuestionOrderBy>{}

    public class QuestionFilter : ConditionContainer
    {
        public string SearchTerm;
        public int CreatorId = -1;
        public int ValuatorId = -1;
    }

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

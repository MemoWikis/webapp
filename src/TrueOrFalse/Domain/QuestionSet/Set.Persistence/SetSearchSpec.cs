using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    [Serializable]
    public class SetSearchSpec : SearchSpecificationBase<SetFilter, SetOrderBy>
    {
        public string SearchTerm;

        public SpellCheckResult SpellCheck;

        public string GetSuggestion()
        {
            return SpellCheck.GetSuggestion();
        }
    }

    [Serializable]
    public class SetFilter : ConditionContainer
    {
        public ConditionInteger CreatorId;
        public int ValuatorId = -1;

        public SetFilter(){
            CreatorId = new ConditionInteger(this, "Creator.Id");
        }
    }

    [Serializable]
    public class SetOrderBy : OrderByCriteria
    {
        public OrderBy CreationDate;
        public OrderBy ValuationsAvg;
        public OrderBy ValuationsCount;

        public SetOrderBy()
        {
            CreationDate = new OrderBy("DateCreated", this);
            ValuationsCount = new OrderBy("TotalRelevancePersonalEntries", this);
            ValuationsAvg = new OrderBy("TotalRelevancePersonalAvg", this);
        }

        public string ToText()
        {
            if (CreationDate.IsCurrent())
                return "Erstellungsdatum";

            if (ValuationsCount.IsCurrent())
                return "Anzahl Gemerkt";

            if (ValuationsAvg.IsCurrent())
                return "Gemerkt &#216; Wichtigkeit";

            return "";
        }
    }
}

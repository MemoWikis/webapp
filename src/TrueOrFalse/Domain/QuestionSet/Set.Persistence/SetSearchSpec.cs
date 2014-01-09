using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    public class SetSearchSpec : SearchSpecificationBase<SetFilter, SetOrderBy>
    {
        public string SearchTerm;
    }

    public class SetFilter : ConditionContainer
    {
        public ConditionInteger CreatorId;
        public int ValuatorId = -1;

        public SetFilter(){
            CreatorId = new ConditionInteger(this, "Creator.Id");
        }
    }

    public class SetOrderBy : OrderByCriteria
    {
        public OrderBy ValuationsAvg;
        public OrderBy ValuationsCount;

        public OrderBy CreationDate;

        public SetOrderBy()
        {
            CreationDate = new OrderBy("DateCreated", this);
            ValuationsCount = new OrderBy("TotalRelevancePersonalEntries", this);
            ValuationsAvg = new OrderBy("TotalRelevancePersonalAvg", this);
        }
    }
}

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
    public class SetSearchSpec : SearchSpecificationBase<QuestionSetFilter, QuestionSetOrderBy>
    {
        public string SearchTearm;
    }

    public class QuestionSetFilter : ConditionContainer
    {
        public ConditionInteger CreatorId;

        public QuestionSetFilter(){
            CreatorId = new ConditionInteger(this, "Creator.Id");
        }
    }

    public class QuestionSetOrderBy : OrderByCriteria
    {
        public OrderBy OrderByCreationDate;

        public QuestionSetOrderBy()
        {
            OrderByCreationDate = new OrderBy("DateCreated", this);
        }
    }
}

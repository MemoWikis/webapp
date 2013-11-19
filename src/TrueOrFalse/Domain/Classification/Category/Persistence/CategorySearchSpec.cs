using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    public class CategorySearchSpec : SearchSpecificationBase<CategoryFilter, CategorySetOrderBy>
    {
        public string SearchTerm;

        public bool FilterByMe { get; private set; }
        public bool FilterByAll { get; private set; }
        public ReadOnlyCollection<int> FilterByUsers { get; private set; }

        public CategorySearchSpec()
        {
            FilterByUsers = new ReadOnlyCollection<int>(new List<int>());
            FilterByMe = true;
            FilterByAll = true;
            UpdateUserFilter();
        }

        public void SetFilterByMe(bool? value)
        {
            if (!value.HasValue || value.Value == FilterByMe) return;
            FilterByMe = value.Value;
            UpdateUserFilter();
        }

        public void SetFilterByAll(bool? value)
        {
            if (!value.HasValue || value.Value == FilterByAll) return;
            FilterByAll = value.Value;
            UpdateUserFilter();
        }

        private void UpdateUserFilter()
        {
            Filter.Clear();

            if (FilterByAll && !FilterByMe)
            {
                var condition = new ConditionInteger(Filter, "Creator.Id");
                condition.IsNotEqualTo(ServiceLocator.Resolve<SessionUser>().User.Id);
            }
            else if (FilterByAll && FilterByMe)
            {
                // show all, not filtered
            }
            else
            {
                var condition = new ConditionDisjunction<int>(Filter, "Creator.Id");
                condition.Add(FilterByUsers.ToArray());
                if (FilterByMe)
                {
                    condition.Add(ServiceLocator.Resolve<SessionUser>().User.Id);
                }
                if (condition.ItemCount == 0)
                {
                    condition.Add(-1);
                }
            }
        }
    }

    public class CategoryFilter : ConditionContainer
    {
    }

    public class CategorySetOrderBy : OrderByCriteria
    {
        public OrderBy CreationDate;
        public OrderBy QuestionCount;

        public CategorySetOrderBy()
        {
            CreationDate = new OrderBy("DateCreated", this);
            QuestionCount = new OrderBy("CountQuestions", this);
        }
    }
}

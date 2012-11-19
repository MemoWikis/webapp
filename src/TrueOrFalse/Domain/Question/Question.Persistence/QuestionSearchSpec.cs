using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    public class QuestionSearchSpec : SearchSpecificationBase<QuestionFilter, QuestionOrderBy>
    {
        public string SearchTearm;

        public bool FilterByMe { get; private set; }
        public bool FilterByAll { get; private set; }
        public ReadOnlyCollection<int> FilterByUsers { get; private set; }

        public QuestionSearchSpec()
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
        
        public void AddFilterByUser(int? userId)
        {
            if (!userId.HasValue) return;
            var newUserIds = (FilterByUsers ?? Enumerable.Empty<int>()).Union(new[] { userId.Value }).ToList().AsReadOnly();
            if (FilterByUsers != null && newUserIds.SequenceEqual(FilterByUsers)) return;

            FilterByUsers = newUserIds;
            UpdateUserFilter();
        }

        public void DelFilterByUser(int? userId)
        {
            if (!userId.HasValue) return;
            var newUserIds = (FilterByUsers ?? Enumerable.Empty<int>()).Except(new[] { userId.Value }).ToList().AsReadOnly();
            if (FilterByUsers != null && newUserIds.SequenceEqual(FilterByUsers)) return;

            FilterByUsers = newUserIds;
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

    public class QuestionFilter : ConditionContainer
    {
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

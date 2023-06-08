using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

[Serializable]
public class CategorySearchSpec : SearchSpecificationBase<CategoryFilter, CategorytOrderBy>
{
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

    private void UpdateUserFilter()
    {
        Filter.Clear();

        if (FilterByAll && !FilterByMe)
        {
            var condition = new ConditionInteger(Filter, "Creator.Id");
            condition.IsNotEqualTo(SessionUserLegacy.UserId);
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
                condition.Add(SessionUserLegacy.UserId);
            }
            if (condition.ItemCount == 0)
            {
                condition.Add(-1);
            }
        }
    }
}

[Serializable]
public class CategoryFilter : ConditionContainer
{
    public int ValuatorId = -1;
}

[Serializable]
public class CategorytOrderBy : OrderByCriteria
{
    public OrderBy BestMatch;
    public OrderBy CreationDate;
    public OrderBy QuestionCount;
}
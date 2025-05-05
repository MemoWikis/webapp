[Serializable]
public class SettingOrderBy : OrderByCriteria;


[Serializable]
public class SettingSearchDesc : Pager, ISearchDesc
{
    private SettingSearchFilter _filter = new SettingSearchFilter();
    public SettingSearchFilter Filter { get { return _filter ?? (_filter = new SettingSearchFilter()); } }

    private readonly SettingOrderBy _orderBy = new SettingOrderBy();
    public SettingOrderBy OrderBy { get { return _orderBy; } }

    ConditionContainer ISearchDesc.Filter { get { return Filter; } }
    OrderByCriteria ISearchDesc.OrderBy { get { return OrderBy; } }
}
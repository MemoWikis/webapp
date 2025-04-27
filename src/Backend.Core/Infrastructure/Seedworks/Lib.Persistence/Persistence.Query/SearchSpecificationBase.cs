/// <summary>
/// Base class for specifications
/// </summary>
/// <typeparam name="TFilter"></typeparam>
/// <typeparam name="TOrderBy"></typeparam>
[Serializable]
public class SearchSpecificationBase<TFilter, TOrderBy> : Pager, ISearchDesc
    where TFilter : ConditionContainer, new()
    where TOrderBy : OrderByCriteria, new()
{
    ConditionContainer ISearchDesc.Filter => Filter;

    OrderByCriteria ISearchDesc.OrderBy => OrderBy;

    private readonly TFilter _filter = new TFilter();
    private readonly TOrderBy _orderBy = new TOrderBy();

    public TFilter Filter => _filter;

    public TOrderBy OrderBy => _orderBy;
}
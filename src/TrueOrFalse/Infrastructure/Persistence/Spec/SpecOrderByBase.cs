using Seedworks.Lib.Persistence;

[Serializable]
public class SpecOrderByBase : OrderByCriteria
{
    public OrderBy Created;
    public OrderBy Modified;

    public SpecOrderByBase()
    {
        Created = new OrderBy("DateCreated", this);
        Modified = new OrderBy("DateModified", this);
    }
}
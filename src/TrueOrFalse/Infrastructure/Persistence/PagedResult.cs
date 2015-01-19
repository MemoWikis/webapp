using System.Collections.Generic;


public class PagedResult<T>
{
    public int PageSize;
    public int Total;
    public IList<T> Items = new List<T>();
}
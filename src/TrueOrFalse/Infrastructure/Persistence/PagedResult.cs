using System.Collections.Generic;

namespace TrueOrFalse.Infrastructure.Persistence
{
    public class PagedResult<T>
    {
        public int PageSize;
        public int Total;
        public IList<T> Items = new List<T>();
    }
}

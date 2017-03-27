using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

public class ReferenceCount
{
    public static int Get(int categorId)
    {
        return Sl.Session
            .QueryOver<Reference>()
            .Where(x => x.Category.Id == categorId)
            .RowCount();
    }
}
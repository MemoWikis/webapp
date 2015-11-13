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

    public static IList<ReferenceCountPair> GetList(List<int> categoryIds)
    {
        return Sl.Session
            .QueryOver<Reference>()
            .Where(Restrictions.In("Category.Id", categoryIds))
            .Select(
                Projections.Group<Reference>(x => x.Category.Id),
                Projections.Count<Reference>(x => x.Category.Id)
            )
            .List<object[]>()
            .Select(p => new ReferenceCountPair()
            {
                CategoryId = (int)p[0],
                ReferenceCount = (int)p[1]
            })
            .ToList();
    }
}

public class ReferenceCountPair
{
    public int CategoryId;
    public int ReferenceCount;
}
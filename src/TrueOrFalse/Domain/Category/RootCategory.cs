using System.Collections.Generic;
using NHibernate.Util;
using System.Linq;

public class RootCategory
{
    public const int RootCategoryId = 1;
    public static CategoryCacheItem Get => EntityCache.GetCategoryCacheItem(RootCategoryId);
    public static IReadOnlyList<int> LockedCategoryIds = new List<int> {1, 682, 689, 709};
    public static bool IsMainCategory(int categoryId) => LockedCategoryIds.Any(c => c == categoryId);

}
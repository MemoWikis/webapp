using System.Collections.Generic;
using System.Linq;

public class RootCategory
{
    public const int RootCategoryId = 1;
    public static CategoryCacheItem Get => EntityCache.GetCategoryCacheItem(RootCategoryId, getDataFromEntityCache: true);
    public static IReadOnlyList<int> LockedCategoryIds => GetLockedCategoryIds();
    public static bool LockedCategory(int categoryId) => LockedCategoryIds.Any(c => c == categoryId);

    public const int IntroCategoryId = 1864;
    public const int MemuchoWikiId = 1890;

    public static IList<int> MainCategoryIds = new List<int> { 682, 687, 689, 709 };
    public static IList<int> PopularCategoryIds = new List<int> { 269, 153, 266, 388, 680 };
    public static IList<int> MemuchoCategoryIds = new List<int> { 1876, 8975, 8974 };
    public static IList<int> MemuchoHelpIds = new List<int> { 1864, 9002 };

    private static IReadOnlyList<int> GetLockedCategoryIds()
    {
        var list = new List<int>();
        list.Add(RootCategoryId);
        list.AddRange(MainCategoryIds);
        list.Add(MemuchoWikiId);
        list.AddRange(MemuchoCategoryIds);
        list.AddRange(MemuchoHelpIds);
        return list;
    }
}
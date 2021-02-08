public class RootCategory
{
    public const int RootCategoryId = 1;
    public static Category Get => EntityCache.GetCategory(RootCategoryId);
}
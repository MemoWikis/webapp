using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ImageMetaDataCache
{
    public static IDictionary<int, ImageMetaData> RequestCache_Questions => GetRequestItemsCache("imageMetaDatasQuestion");
    public static IDictionary<int, ImageMetaData> RequestCache_Categories => GetRequestItemsCache("imageMetaDatasCategories");

    public static bool IsInCache(int typeId, ImageType imageType)
    {
        if (imageType == ImageType.Question)
            if (RequestCache_Questions.ContainsKey(typeId))
                return true;

        if (imageType == ImageType.Category)
            if (RequestCache_Categories.ContainsKey(typeId))
                return true;

        return false;
    }

    public static ImageMetaData FromCache(int typeId, ImageType imageType)
    {
        if (imageType == ImageType.Question)
            return RequestCache_Questions[typeId];

        if (imageType == ImageType.Category)
            return RequestCache_Categories[typeId];

        return null;
    }

    private static IDictionary<int, ImageMetaData> GetRequestItemsCache(string itemsKey)
    {
        var result = HttpContext.Current.Items[itemsKey];

        if (result == null)
            return new Dictionary<int, ImageMetaData>();

        return (IDictionary<int, ImageMetaData>)result;
    }
}
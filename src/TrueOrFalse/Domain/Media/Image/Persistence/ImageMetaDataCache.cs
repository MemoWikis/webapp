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

    private static void SetRequestItemsCache(IList<int> ids , string itemsKey, ImageType imageType)
    {
        ids = ids.Distinct().ToList();

        var items = Sl.Resolve<ImageMetaDataRepo>()
            .GetBy(ids, imageType)
            .ToDictionary(r => r.TypeId);

        var dictionary = new Dictionary<int, ImageMetaData>();
        
        foreach (var id in ids)
        {
            if(items.ContainsKey(id))
                dictionary.Add(id, items[id]);
            else
                dictionary.Add(id, null);
        }

        HttpContext.Current.Items[itemsKey] = dictionary;
    }

    private static IDictionary<int, ImageMetaData> GetRequestItemsCache(string itemsKey)
    {
        var result = HttpContext.Current.Items[itemsKey];

        if (result == null)
            return new Dictionary<int, ImageMetaData>();

        return (IDictionary<int, ImageMetaData>)result;
    }
}
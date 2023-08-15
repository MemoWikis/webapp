using System.Collections.Generic;
using System.Linq;
using System.Web;
using CacheManager.Core;

public class ImageMetaDataCache
{

    private static readonly string _imageMetaDatasQuestionsKey = "imageMetaDatasQuestion"; 
    private static readonly string _imageMetaDatasCategoriesKey = "imageMetaDatasQuestion";
    private static ICacheManager<object>? _cache;
    public static IDictionary<int, ImageMetaData> RequestCache_Questions(ImageMetaDataReadingRepo imageMetaDataReadingRepo) => 
        GetRequestItemsCache(_imageMetaDatasQuestionsKey, ImageType.Question, imageMetaDataReadingRepo);
    public static IDictionary<int, ImageMetaData> RequestCache_Categories(ImageMetaDataReadingRepo imageMetaDataReadingRepo) => 
        GetRequestItemsCache(_imageMetaDatasCategoriesKey, ImageType.Category, imageMetaDataReadingRepo);


    private static void InitCache(string cacheKey, ImageType imageType, ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        if (_cache == null)
        {
            _cache = CacheFactory.Build<object>(cacheKey,
                settings => { settings.WithSystemRuntimeCacheHandle("handleName"); });
            _cache.Put(cacheKey, imageMetaDataReadingRepo.GetAll(imageType));
        }
    }
    public static bool IsInCache(int typeId, ImageType imageType, ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        if (imageType == ImageType.Question)
            if (RequestCache_Questions(imageMetaDataReadingRepo).ContainsKey(typeId))
                return true;

        if (imageType == ImageType.Category)
            if (RequestCache_Categories(imageMetaDataReadingRepo).ContainsKey(typeId))
                return true;

        return false;
    }

    public static ImageMetaData FromCache(int typeId, ImageType imageType,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        if (imageType == ImageType.Question)
            return RequestCache_Questions(imageMetaDataReadingRepo)[typeId];

        if (imageType == ImageType.Category)
            return RequestCache_Categories(imageMetaDataReadingRepo)[typeId];

        return null;
    }

    private static IDictionary<int, ImageMetaData> GetRequestItemsCache(string cacheKey,
        ImageType imageType, 
        ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        InitCache(cacheKey, imageType, imageMetaDataReadingRepo);
        return _cache.Get<IDictionary<int, ImageMetaData>>(cacheKey);
    }
}
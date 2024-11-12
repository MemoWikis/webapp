using System.Collections.Concurrent;

public class ImageMetaDataCache
{

    private static readonly string _imageMetaDataQuestionsKey = "imageMetaDatasQuestion";
    private static readonly string _imageMetaDataPagesKey = "imageMetaDatasCategories";

    public static IDictionary<(int, int), ImageMetaData> RequestCache_Questions(ImageMetaDataReadingRepo imageMetaDataReadingRepo) =>
        GetRequestItemsCache(_imageMetaDataQuestionsKey, imageMetaDataReadingRepo);
    public static IDictionary<(int, int), ImageMetaData> RequestCache_Pages(ImageMetaDataReadingRepo imageMetaDataReadingRepo) =>
        GetRequestItemsCache(_imageMetaDataPagesKey, imageMetaDataReadingRepo);


    public static bool IsInCache(int typeId, ImageType imageType, ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        if (imageType == ImageType.Question)
            if (RequestCache_Questions(imageMetaDataReadingRepo).ContainsKey((typeId, (int)imageType)))
                return true;

        if (imageType == ImageType.Page)
            if (RequestCache_Pages(imageMetaDataReadingRepo).ContainsKey((typeId, (int)imageType)))
                return true;

        return false;
    }

    public static ImageMetaData? FromCache(int typeId, ImageType imageType,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        if (imageType == ImageType.Question)
        {
            return RequestCache_Questions(imageMetaDataReadingRepo)[(typeId, (int)imageType)];
        }

        if (imageType == ImageType.Page)
        {
            return RequestCache_Pages(imageMetaDataReadingRepo)[(typeId, (int)imageType)];
        }

        return null;
    }

    private static IDictionary<(int, int), ImageMetaData> GetRequestItemsCache(string cacheKey,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {

        var cache = (ConcurrentDictionary<(int, int), ImageMetaData>)Cache.Mgr.Get(cacheKey);
        if (cache == null || cache.Any() == false)
        {
            var metadata = imageMetaDataReadingRepo.GetAll();
            Cache.IntoForeverCache(cacheKey, metadata.ToConcurrentDictionary());
        }

        return Cache.Mgr.Get<IDictionary<(int, int), ImageMetaData>>(cacheKey);
    }
}
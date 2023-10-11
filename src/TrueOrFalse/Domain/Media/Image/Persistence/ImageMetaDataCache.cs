using System.Collections.Concurrent;

public class ImageMetaDataCache : BaseEntityCache
{

    private static readonly string _imageMetaDatasQuestionsKey = "imageMetaDatasQuestion";
    private static readonly string _imageMetaDatasCategoriesKey = "imageMetaDatasCategories";

    public static IDictionary<(int, int), ImageMetaData> RequestCache_Questions(ImageMetaDataReadingRepo imageMetaDataReadingRepo) =>
        GetRequestItemsCache(_imageMetaDatasQuestionsKey, imageMetaDataReadingRepo);
    public static IDictionary<(int, int), ImageMetaData> RequestCache_Categories(ImageMetaDataReadingRepo imageMetaDataReadingRepo) =>
        GetRequestItemsCache(_imageMetaDatasCategoriesKey, imageMetaDataReadingRepo);


    public static bool IsInCache(int typeId, ImageType imageType, ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        ICollection<ImageMetaData> questionImageMetaDataCache = null;
        if (imageType == ImageType.Question)
        {
            questionImageMetaDataCache = RequestCache_Questions(imageMetaDataReadingRepo).Values;

        }
        if (imageType == ImageType.Category)
            if (RequestCache_Categories(imageMetaDataReadingRepo).ContainsKey((typeId, (int)imageType)))
                return true;

        var imagemetaData = questionImageMetaDataCache.FirstOrDefault(qimd => qimd.Type == imageType && qimd.TypeId == typeId);
        if (imagemetaData != null)
            return true;

        return false;
    }

    public static ImageMetaData? FromCache(int typeId, ImageType imageType,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {
        ICollection<ImageMetaData> imageMetaDataCache = null;
        if (imageType == ImageType.Question)
        {
            return RequestCache_Questions(imageMetaDataReadingRepo)[(typeId, (int)imageType)];
        }

        if (imageType == ImageType.Category)
        {
            return RequestCache_Questions(imageMetaDataReadingRepo)[(typeId,(int)imageType)];
        }

        return null;
    }

    private static IDictionary<(int, int), ImageMetaData> GetRequestItemsCache(string cacheKey,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo)
    {

        var cache = (ConcurrentDictionary<(int, int), ImageMetaData>)_cache.Get(cacheKey);
        if (cache == null || cache.Any() == false)
        {
            var metadata = imageMetaDataReadingRepo.GetAll();
            IntoForeverCache(cacheKey, metadata.ToConcurrentDictionary());
        }

        return _cache.Get<IDictionary<(int, int), ImageMetaData>>(cacheKey);
    }

    private static ImageType GetImageType(string key)
    {
        if (key.Equals(_imageMetaDatasQuestionsKey))
            return ImageType.Question;
        else if (key.Equals(_imageMetaDatasCategoriesKey))
            return ImageType.Category;

        throw new InvalidOperationException("Type not found");
    }
}
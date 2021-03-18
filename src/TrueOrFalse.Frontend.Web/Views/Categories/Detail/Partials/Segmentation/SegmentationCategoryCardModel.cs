using System.Linq;

public class SegmentationCategoryCardModel : BaseContentModule
{
    public CategoryCacheItem Category;
    public string Title;

    public SegmentationCategoryCardModel(CategoryCacheItem category)
    {
        Category = category; 
        Title = category.Name;
    }

    public int GetTotalQuestionCount(CategoryCacheItem category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public ImageFrontendData GetCategoryImage(CategoryCacheItem category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }
}







using System.Linq;

public class SegmentationCategoryCardModel : BaseContentModule
{
    public Category Category;
    public string Title;

    public SegmentationCategoryCardModel(Category category)
    {
        Category = category; 
        Title = category.Name;
    }

    public int GetTotalQuestionCount(Category category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }
}







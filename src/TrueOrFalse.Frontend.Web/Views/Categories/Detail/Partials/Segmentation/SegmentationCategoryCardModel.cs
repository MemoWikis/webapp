using System;
using System.Collections.Generic;
using System.Linq;

public class SegmentationCategoryCardModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public SegmentationCategoryCardModel(Category category)
    {
        Category = category;

        var isLoadList = false;
        var categoryList = UserCache.IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId) : Sl.CategoryRepo.GetChildren(category.Id).ToList();

        CategoryList = categoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();
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

    public int GetTotalTopicCount(Category category)
    {
        return EntityCache.GetChildren(category.Id).Count(c => c.Type == CategoryType.Standard && c.GetAggregatedQuestionIdsFromMemoryCache().Count > 0);

    }
}







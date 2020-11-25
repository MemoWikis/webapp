using System;
using System.Security.Policy;

public class KnowledgeCardMiniCategoryModel:BaseModel
{
    public Category Category;
    public bool isInWishKnowledge;

    public KnowledgeCardMiniCategoryModel(Category category)
    {
        Category = category;
        isInWishKnowledge = Sl.CategoryValuationRepo.IsInWishKnowledge(Category.Id, UserId);
    }

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    public int GetTotalQuestionCount(Category category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }

    //public int GetTotalSetCount(Category category)
    //{
    //    return category.GetAggregatedSetsFromMemoryCache().Count;
    //}
}
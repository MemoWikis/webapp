using System;

public class KnowledgeCardMiniCategoryModel
{
    public Category Category;
    public KnowledgeCardMiniCategoryModel(Category category)
    {
        Category = category;
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

    public int GetTotalSetCount(Category category)
    {
        return category.GetAggregatedSetsFromMemoryCache().Count;
    }

    public string getLinktoLearn()
    {
        
        return "Fragesatz/Lernen/"; // + category.AggregatedCategories()[0].Id;
    }
}
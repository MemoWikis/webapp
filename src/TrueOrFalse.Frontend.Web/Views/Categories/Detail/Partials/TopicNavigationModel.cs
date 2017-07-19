using System.Collections.Generic;
using System.Linq;

public class TopicNavigationModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> TopicList;


    public TopicNavigationModel(Category category, string title, string text, List<int> topicIdList)
    {
        TopicList = 
            topicIdList != null
            ? ConvertToCategoryList(topicIdList)
            : Sl.CategoryRepo.GetChildren(category.Id).ToList();

        Title = title;
        Text = text;
    }

    public int GetTotalQuestionCount(Category category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public int GetTotalSetCount(Category category)
    {
        return category.GetAggregatedSetsFromMemoryCache().Count;
    }

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    private List<Category> ConvertToCategoryList(List<int> categoryIds)
    {
        var categoryList = new List<Category>();
        foreach (var subCategoryId in categoryIds)
        {
            //TODO:Julian FEHLER BEHANDELUNG BEI NULL REFERENCE CATEGORY ID
            var category = Sl.CategoryRepo.GetById(subCategoryId);
            categoryList.Add(category);
        }

        return categoryList;
    }

    //private void OrderTopicList(List<int> topicIdOrderList)
    //{
    //    if (topicIdOrderList != null)
    //    {
    //        var topicOrderList = ConvertToCategoryList(topicIdOrderList);

    //        foreach (var category in topicOrderList)
    //        {
    //            TopicList.Remove(category);
    //        }

    //        //TODO:Julian COULD GET WRONG CATEGORIES INTO LIST
    //        topicOrderList.AddRange(TopicList);
    //        TopicList = topicOrderList;
    //    }
    //}
}
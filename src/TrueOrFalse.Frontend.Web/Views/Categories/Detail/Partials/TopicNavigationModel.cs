using System.Collections.Generic;
using System.Linq;

public class TopicNavigationModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;


    public TopicNavigationModel(Category category, string title, string text, List<int> categoryIdList, string orderType)
    {
        Category = category;

        CategoryList = 
            categoryIdList != null
            ? ConvertToCategoryList(categoryIdList)
            : Sl.CategoryRepo.GetChildren(category.Id).ToList();

        if (categoryIdList != null && orderType == "SelectionFirst")
        {
            AppendRemainingItemsToList();
        }

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

    private List<Category> ConvertToCategoryList(List<int> categoryIdList)
    {
        var categoryList = new List<Category>();
        foreach (var categoryId in categoryIdList)
        {
            //TODO:Julian FEHLER BEHANDELUNG BEI NULL REFERENCE CATEGORY ID
            var category = Sl.CategoryRepo.GetById(categoryId);
            categoryList.Add(category);
        }

        return categoryList;
    }

    private void AppendRemainingItemsToList()
    {
        var totalSubCategories = Sl.CategoryRepo.GetChildren(Category.Id).ToList();
        foreach (var subCategory in CategoryList)
        {
            totalSubCategories.Remove(subCategory);
        }

        CategoryList.AddRange(totalSubCategories);
    }
}
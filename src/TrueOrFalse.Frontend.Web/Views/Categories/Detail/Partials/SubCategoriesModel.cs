using System.Collections.Generic;
using System.Linq;

public class SubCategoriesModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> SubCategoryList;


    public SubCategoriesModel(Category category, string title, string text, List<int> subCategoryIdList, List<int> subCategoryIdOrderList)
    {
        SubCategoryList = 
            subCategoryIdList != null
            ? ConvertToCategoryList(subCategoryIdList)
            : Sl.CategoryRepo.GetChildren(category.Id).ToList();

        Title = title;
        Text = text;

        OrderSubCategoryList(subCategoryIdOrderList);
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

    private void OrderSubCategoryList(List<int> subCategoryIdOrderList)
    {
        if (subCategoryIdOrderList != null)
        {
            var subCategoryOrderList = ConvertToCategoryList(subCategoryIdOrderList);

            foreach (var category in subCategoryOrderList)
            {
                SubCategoryList.Remove(category);
            }

            //TODO:Julian COULD GET WRONG CATEGORIES INTO LIST
            subCategoryOrderList.AddRange(SubCategoryList);
            SubCategoryList = subCategoryOrderList;
        }
    }
}
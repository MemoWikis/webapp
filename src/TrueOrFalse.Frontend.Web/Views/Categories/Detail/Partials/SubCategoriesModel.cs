using System.Collections.Generic;

public class SubCategoriesModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public IList<Category> SubCategoryList;


    public SubCategoriesModel(Category category, string title, string text, List<int> subCategoryIdList)
    {
        SubCategoryList = 
            subCategoryIdList != null
            ? ConvertToSubCategoryList(subCategoryIdList)
            : Sl.CategoryRepo.GetChildren(category.Id);

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

    private List<Category> ConvertToSubCategoryList(List<int> subCategoryIds)
    {
        var subCategoryList = new List<Category>();
        foreach (var subCategoryId in subCategoryIds)
        {
            //TODO:Julian FEHLER BEHANDELUNG BEI NULL REFERENCE CATEGORY ID
            var subCategory = Sl.CategoryRepo.GetById(subCategoryId);
            subCategoryList.Add(subCategory);
        }

        return subCategoryList;
    }
}
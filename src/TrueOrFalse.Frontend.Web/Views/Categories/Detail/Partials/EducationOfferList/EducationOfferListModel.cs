using System;
using System.Collections.Generic;
using System.Linq;

public class EducationOfferListModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public bool HasUsedOrderListWithLoadList;

    public EducationOfferListModel(Category category, EducationOfferListJson educationOfferListJson)
    {
        Category = category;

        var isLoadList = false;
        switch (educationOfferListJson.Load)
        {
            case null:
            case "All":
                CategoryList = Sl.CategoryRepo.GetChildren(category.Id).Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education).ToList();
                break;

            default:
                var categoryIdList = educationOfferListJson.Load.Split(',').ToList().ConvertAll(Int32.Parse);
                CategoryList = ConvertToCategoryList(categoryIdList);
                isLoadList = true;
                break;
        }

        switch (educationOfferListJson.Order)
        {
            case null:
            case "QuestionAmount":
                if(educationOfferListJson.Load == null || educationOfferListJson.Load == "All")
                    CategoryList = CategoryList.OrderByDescending(c => c.GetAggregatedQuestionsFromMemoryCache().Count).ToList();
                break;

            case "Name":
                CategoryList = CategoryList.OrderBy(c => c.Name).ToList();
                break;

            default:
                if (isLoadList)
                {
                    throw new Exception("\"Load: \" und \"Order: \" können nicht gleichzeitig mit Category-Id-Listen als Parameter verwendet werden!");
                }
                var firstCategories = ConvertToCategoryList(educationOfferListJson.Order.Split(',').ToList().ConvertAll(Int32.Parse));
                CategoryList = OrderByCategoryList(firstCategories);
                break;
        }



        Title = educationOfferListJson.Title ?? "Bildungsangebote";
        Text = educationOfferListJson.Text;
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

    private List<Category> OrderByCategoryList(List<Category> firstCategories)
    {
        foreach (var category in firstCategories)
        {
            CategoryList.Remove(category);
        }

        firstCategories.AddRange(CategoryList);
        return firstCategories;
    }
}
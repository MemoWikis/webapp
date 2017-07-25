using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class TopicNavigationModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public bool HasUsedOrderListWithLoadList;

    public TopicNavigationModel(Category category, string title, string text, string load, string order)
    {
        Category = category;

        var isLoadList = false;
        switch (load)
        {
            case null:
            case "All":
                CategoryList = Sl.CategoryRepo.GetChildren(category.Id).ToList();
                break;

            default:
                var categoryIdList = load.Split(',').ToList().ConvertAll(Int32.Parse);
                CategoryList = ConvertToCategoryList(categoryIdList);
                isLoadList = true;
                break;
        }

        switch (order)
        {
            case null:
                break;

            default:
                if (isLoadList)
                {
                    HasUsedOrderListWithLoadList = true;
                    break;
                }
                var firstCategories = ConvertToCategoryList(order.Split(',').ToList().ConvertAll(Int32.Parse));
                CategoryList = OrderByCategoryList(firstCategories);
                break;
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
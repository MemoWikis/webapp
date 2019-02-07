using System;
using System.Net.Mime;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;

public class SingleCategoryModel : BaseContentModule
{
    public int CategoryId;
    public string CategoryName;
    public string CategoryText;
    public int QCount; //Number of questions

    public ImageFrontendData ImageFrontendData;

    public SingleCategoryModel(int categoryId, string categoryText = null)
    {
        var category = Resolve<CategoryRepository>().GetById(categoryId) ?? new Category();
        
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        CategoryId = category.Id;
        CategoryName = category.Name;
        CategoryText = categoryText ?? category.Description;

        QCount = category.CountQuestionsAggregated;
    }

    public static CardSingleCategoryModel GetCardSingleCategoryModel(int categoryId, string categoryText = null)
    {
        return new CardSingleCategoryModel(categoryId, categoryText);
    }
}

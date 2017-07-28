using System;
using System.Net.Mime;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;

public class CardSingleCategoryModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public string CategoryText;
    public int QuestionCount;

    public ImageFrontendData ImageFrontendData;

    public CardSingleCategoryModel(int categoryId, string categoryText = null)
    {
        var category = Resolve<CategoryRepository>().GetById(categoryId) ?? new Category();
        
        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        CategoryId = category.Id;
        CategoryName = category.Name;
        CategoryText = categoryText ?? category.Description;

        QuestionCount = category.CountQuestionsAggregated;
    }

    public static CardSingleCategoryModel GetCardSingleCategoryModel(int categoryId, string categoryText = null)
    {
        return new CardSingleCategoryModel(categoryId, categoryText);
    }
}

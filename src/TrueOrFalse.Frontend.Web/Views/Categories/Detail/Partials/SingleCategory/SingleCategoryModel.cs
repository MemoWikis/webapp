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
    public bool IsInWishknowledge;
    public Category Category; 


    public ImageFrontendData ImageFrontendData;

    public SingleCategoryModel(SingleCategoryJson singleCategoryJson)
    {
        var category = Resolve<CategoryRepository>().GetById(singleCategoryJson.CategoryId) ?? new Category();
        
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        CategoryId = category.Id;
        CategoryName = category.Name;
        CategoryText = singleCategoryJson.Description ?? category.Description;

        QCount = category.CountQuestionsAggregated;
    }

    public SingleCategoryModel(int categoryId)
    {
        Category = Resolve<CategoryRepository>().GetById(categoryId) ?? new Category();

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(Category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        CategoryId = Category.Id;
        CategoryName = Category.Name;
        CategoryText = Category.Description ?? Category.Description;
        IsInWishknowledge = R<SetValuationRepo>().GetBy(categoryId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;

        QCount = Category.CountQuestionsAggregated;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class WelcomeCardMiniCategoryModel : BaseModel
{
    public Category Category;
    public int CategoryId;
    public string CategoryName;
    public string CategoryDescription;
    public ImageFrontendData ImageFrontendData;

    public int QuestionCount;
    public bool IsInWishknowledge;


    public WelcomeCardMiniCategoryModel(int categoryId)
    {
        var category = R<CategoryRepository>().GetById(categoryId) ?? new Category();
        //Category = category;
        CategoryId = categoryId;
        CategoryName = category.Name;

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

    }
}

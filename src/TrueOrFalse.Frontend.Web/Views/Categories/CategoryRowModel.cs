using System;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryRowModel : BaseModel
{
    public int QuestionCount;
    public int CategoryId;
    public string CategoryName;
    public object DescriptionShort;

    public Func<UrlHelper, string> DetailLink;
    public bool UserCanEdit;

    public ImageFrontendData ImageFrontendData;

    public string DateCreated;
    public string DateCreatedLong;

    public CategoryRowModel(Category category, int indexInResultSet)
    {
        CategoryId = category.Id;
        CategoryName = category.Name;
        DescriptionShort = "";
        QuestionCount = category.CountQuestions;

        UserCanEdit = _sessionUser.IsInstallationAdmin;

        DetailLink = urlHelper => Links.CategoryDetail(category.Name, category.Id, indexInResultSet);

        DateCreated = category.DateCreated.ToString("dd.MM.yyyy");
        DateCreatedLong = category.DateCreated.ToString("U");//Change to "g" format?

        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);
    }
}
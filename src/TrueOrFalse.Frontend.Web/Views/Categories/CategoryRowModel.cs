using System;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryRowModel : BaseModel
{
    public string ImageUrl;
    public int QuestionCount;
    public int CategoryId;
    public string CategoryName;
    public object DescriptionShort;

    public Func<UrlHelper, string> DetailLink;
    public bool UserCanEdit;

    public string DateCreated;
    public string DateCreatedLong;

    public CategoryRowModel(Category category, int indexInResultSet)
    {
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_128px_square().Url;
        CategoryId = category.Id;
        CategoryName = category.Name;
        DescriptionShort = "";
        QuestionCount = category.CountQuestions;

        UserCanEdit = _sessionUser.IsInstallationAdmin;

        DetailLink = urlHelper => Links.CategoryDetail(category.Name, category.Id, indexInResultSet);

        DateCreated = category.DateCreated.ToString("dd.MM.yyyy");
        DateCreatedLong = category.DateCreated.ToString("U");
    }
}
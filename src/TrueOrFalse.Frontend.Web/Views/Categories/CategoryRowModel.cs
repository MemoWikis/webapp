using System;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;

public class CategoryRowModel : BaseModel
{
    public string ImageUrl;
    public int QuestionCount;
    public int CategoryId;
    public string CategoryName;
    public object DescriptionShort;

    public Func<UrlHelper, string> DetailLink;
    public bool UserCanEdit;

    public CategoryRowModel(Category category, int indexInResultSet)
    {
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_85px_square().Url;
        CategoryId = category.Id;
        CategoryName = category.Name;
        DescriptionShort = "";
        QuestionCount = category.QuestionCount;

        UserCanEdit = _sessionUser.IsInstallationAdmin;

        DetailLink = urlHelper => Links.CategoryDetail(urlHelper, category.Name, category.Id, indexInResultSet);
    }
}
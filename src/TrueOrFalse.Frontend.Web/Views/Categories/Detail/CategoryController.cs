using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using NHibernate.Criterion;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";

    [SetMenu(MenuEntry.CategoryDetail)]
    public ActionResult Category(string text, int id, int elementOnPage)
    {
        var category = Resolve<CategoryRepository>().GetById(id);
        return Category(category);
    }

    private ActionResult Category(Category category)
    {
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category));
        return View(_viewLocation, new CategoryModel(category));
    }

    public void ByName()
    {
        var name = HttpUtility.UrlDecode((Request["name"]));
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetByName(name)));
    }

}

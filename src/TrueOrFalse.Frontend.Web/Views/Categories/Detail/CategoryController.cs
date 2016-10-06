using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";

    [SetMenu(MenuEntry.CategoryDetail)]
    public ActionResult Category(string text, int id)
    {
        var category = Resolve<CategoryRepository>().GetById(id);
        return Category(category);
    }

    private ActionResult Category(Category category)
    {
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category));
        return View(_viewLocation, new CategoryModel(category));
    }

    public void ById()
    {
        var id = Convert.ToInt32(Request["id"]);
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetById(id)));
    }

}

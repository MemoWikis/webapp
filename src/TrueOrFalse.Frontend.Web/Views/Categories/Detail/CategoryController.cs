using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Criterion;
using TrueOrFalse;

public class CategoryController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";

    [SetMenu(MenuEntry.CategoryDetail)]
    public ActionResult Category(string text, int id, int elementOnPage)
    {
        var category = Resolve<CategoryRepository>().GetById(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category));

        return View(_viewLocation, new CategoryModel(category));
    }


}

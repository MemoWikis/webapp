using System;
using System.Collections.Generic;
using System.Linq;
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

        return View(_viewLocation,
            new CategoryModel(category)
            {
              ContentHtml = MarkdownToHtml.Run(category.TopicMarkdown, ControllerContext)
            });
    }

    public void CategoryById(int id)
    {
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetById(id)));
    }

    public ActionResult StartTestSession(int categoryId)
    {
        var category = Sl.R<CategoryRepository>().GetById(categoryId);
        var testSession = new TestSession(category);

        R<SessionUser>().AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }
}
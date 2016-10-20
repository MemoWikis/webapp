using System;
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
        return View(_viewLocation, new CategoryModel(category));
    }

    public void CategoryById(int id)
    {
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetById(id)));
    }

    public ActionResult StartTestSession(int categoryId)
    {
        var excludeQuestionIds = _sessionUser.AnsweredQuestionIds.ToList();
        var category = Sl.R<CategoryRepository>().GetById(categoryId);
        var testSession = new TestSession(category, excludeQuestionIds);
        if (testSession.QuestionIds.Count == 0)
            throw new Exception("Cannot start TestSession from set with no questions.");

        Sl.R<SessionUser>().TestSession = testSession;
        return Redirect(Links.TestSession());
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMainMenu(MainMenuEntry.CategoryDetail)]
    [SetThemeMenu(true)]
    public void QuestionSet(string text, int id)
    {
        var category = EntityCache.GetCategory(id);
        var categoryChanges = Sl.CategoryChangeRepo.GetForCategory(category.Id);
        var isDeleted = categoryChanges.Any(c => c.Category.Id == category.Id && c.Type == CategoryChangeType.Delete);
        if (isDeleted)
        {
            var baseSetId = Sl.SetRepo.GetById(id).CopiedFrom.Id;
            category = EntityCache.GetCategory(baseSetId);
        }
        Response.Redirect(Links.CategoryDetailLearningTab(category));
    }

    public void QuestionSetById(int id)
    {
        Response.Redirect(Links.SetDetail(Sl.SetRepo.GetById(id)));
    }

    private void QuestionSet(Set set)
    {
        Response.Redirect(Links.CategoryDetail(Sl.CategoryRepo.GetBySetId(set.Id)));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int setId)
    {
        return Redirect(Links.CategoryDetailLearningTab(EntityCache.GetCategory(setId)));
    }

    public ActionResult StartTestSession(int setId)
    {
        return Redirect(Links.CategoryDetailLearningTab(EntityCache.GetCategory(setId)));
    }

    public string KnowledgeBar(int setId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Sets/Detail/SetKnowledgeBar.ascx",
            new SetKnowledgeBarModel(Sl.SetRepo.GetById(setId)),
            ControllerContext
        );
}
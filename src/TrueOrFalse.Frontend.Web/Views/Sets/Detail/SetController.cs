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
        var category = Sl.CategoryRepo.GetBySetId(id);
        var categoryChanges = Sl.CategoryChangeRepo.GetForCategory(category.Id);
        var isDeleted = categoryChanges.Any(c => c.Category == category && c.Type == CategoryChangeType.Delete);
        if (isDeleted)
        {
            var baseSetId = Sl.SetRepo.GetById(id).CopiedFrom.Id;
            category = Sl.CategoryRepo.GetBySetId(baseSetId);
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

    public JsonResult GetRows(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);

        var sbHtmlRows = new StringBuilder();
        foreach (var rowModel in set.QuestionsInSet)
            sbHtmlRows.Append(
                ViewRenderer.RenderPartialView(
                    "~/Views/Sets/Detail/SetQuestionRowResult.ascx",
                    rowModel,
                    ControllerContext)
            );

        return Json(new {Html = sbHtmlRows.ToString()});
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int setId)
    {
        return Redirect(Links.CategoryDetailLearningTab(Sl.CategoryRepo.GetBySetId(setId)));
    }

    public ActionResult StartTestSession(int setId)
    {
        return Redirect(Links.CategoryDetailLearningTab(Sl.CategoryRepo.GetBySetId(setId)));
    }

    //public ActionResult StartTestSessionForSets(List<int> setIds, string setListTitle)
    //{
    //    var sets = Sl.SetRepo.GetByIdsEager(setIds.ToArray());
    //    var testSession = new TestSession(sets, setListTitle);

    //    Sl.SessionUser.AddTestSession(testSession);

    //    return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    //}

    [HttpPost]
    public JsonResult Copy(int sourceSetId)
    {
        var copiedSetId = R<CopySet>().Run(sourceSetId, UserId);
        var copiedSet = R<SetRepo>().GetById(copiedSetId);
        return new JsonResult
        {
            Data = new
            {
                CopiedSetId = copiedSetId,
                CopiedSetName = copiedSet.Name,
                CopiedSetEditUrl = Links.QuestionSetEdit(copiedSet.Name, copiedSet.Id)
            }
        };
    }

    public string KnowledgeBar(int setId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Sets/Detail/SetKnowledgeBar.ascx",
            new SetKnowledgeBarModel(Sl.SetRepo.GetById(setId)),
            ControllerContext
        );

}
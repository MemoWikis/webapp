using System;
using System.Text;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSet(string text, int id)
    {
        var set = Resolve<SetRepo>().GetByIdEager(id);
        return QuestionSet(set);
    }

    public void QuestionSetById(int id)
    {
        Response.Redirect(Links.SetDetail(Resolve<SetRepo>().GetById(id)));
    }

    private ActionResult QuestionSet(Set set)
    {
        SaveSetView.Run(set, MemuchoUser());

        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set));
        return View(_viewLocation, new SetModel(set));
    }

    public JsonResult GetRows(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);
        var setModel = new SetModel(set);

        var sbHtmlRows = new StringBuilder();
        foreach (var rowModel in setModel.QuestionsInSet)
            sbHtmlRows.Append(
                ViewRenderer.RenderPartialView(
                    "~/Views/Sets/Detail/SetQuestionRowResult.ascx", 
                    rowModel,
                    ControllerContext)
            );
            
        return Json( new { Html = sbHtmlRows.ToString() });
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int setId)
    {
        var set = Resolve<SetRepo>().GetById(setId);
        if (set.Questions().Count == 0)
            throw new Exception("Cannot start LearningSession from set with no questions.");

        var learningSession = new LearningSession{
            SetToLearn = set,
            Steps = GetLearningSessionSteps.Run(set),
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    public ActionResult StartTestSession(int setId)
    {
        var set = Sl.R<SetRepo>().GetById(setId);
        var testSession = new TestSession(set);

        R<SessionUser>().AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }
}
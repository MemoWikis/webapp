using System.Text;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSet(string text, int id)
    {
        return QuestionSetById(id);
    }

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSetById(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);
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

    public ActionResult StartLearningSession(int setId)
    {
        var set = Resolve<SetRepo>().GetById(setId);

        var learningSession = new LearningSession{
            SetToLearn = set,
            Steps = GetLearningSessionSteps.Run(set),
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }
}


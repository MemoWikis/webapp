using System.Linq;
using System.Web.Mvc;
using RabbitMQ.Client.Framing.Impl;
using TrueOrFalse.Frontend.Web.Code;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSet(string text, int id, int elementOnPage)
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

    public ActionResult StartLearningSession(int setId)
    {
        var set = Resolve<SetRepo>().GetById(setId);

        var learningSession = new LearningSession();
        learningSession.Steps = GetLearningSessionSteps.Run(set);
        learningSession.User = _sessionUser.User;
        R<LearningSessionRepo>().Create(learningSession);

        _sessionUser.LearningSession = learningSession;

        return Redirect(Links.LearningSession(learningSession.Id));
    }
}


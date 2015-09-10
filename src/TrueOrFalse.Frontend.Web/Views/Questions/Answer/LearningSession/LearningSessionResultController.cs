using System;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Utils;

public class LearningSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/LearningSession/LearningSessionResult.aspx";

    public ActionResult LearningSessionResult(int learningSessionId, string learningSessionName)
    {
        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        if (learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        learningSession.Steps
            .Where(s => s.AnswerState == StepAnswerState.Uncompleted)
            .Each( s => LearningSessionStep.Skip(s.Id));

        return View(_viewLocation, new LearningSessionResultModel(learningSession));
    }

}

using System;
using System.Web.Mvc;

public class LearningSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/LearningSession/LearningSessionResult.aspx";

    [SetThemeMenu(isLearningSessionPage: true)]
    public ActionResult LearningSessionResult(int learningSessionId, string learningSessionName)
    {
        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        if (learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (!learningSession.IsCompleted)
        {
            learningSession.CompleteSession();
        }

        if (learningSession.IsDateSession)
        {
            TrainingPlanUpdater.Run(learningSession.DateToLearn.TrainingPlan);
        }

        return View(_viewLocation, new LearningSessionResultModel(learningSession));
    }
}

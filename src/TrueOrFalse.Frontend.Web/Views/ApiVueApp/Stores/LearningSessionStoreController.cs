using System.Linq;
using System.Web.Mvc;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class LearningSessionStoreController: BaseController
{
    [HttpPost]
    public JsonResult NewSession(LearningSessionConfig config)
    {
        var learningSession = LearningSessionCreator.BuildLearningSession(config);
        LearningSessionCache.AddOrUpdate(learningSession);
        if (learningSession != null)
            return Json(new
            {
                success = true,
                steps = learningSession.Steps.Select(s => s.AnswerState).ToArray(),
            });

        return Json(new
        {
            success = false
        });
    }
}
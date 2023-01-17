using System.Web.Mvc;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class LearningSessionStoreController: BaseController
{
    [HttpPost]
    public JsonResult NewSession(LearningSessionConfig config)
    {
        LearningSessionCache.AddOrUpdate(LearningSessionCreator.BuildLearningSession(config));
        return Json(new
        {
            success = true
        });
    }
}
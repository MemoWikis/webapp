using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

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
                steps = learningSession.Steps.Select(s => new
                {
                    id = s.Question.Id,
                    state = s.AnswerState
                }).ToArray(),
                activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count()
            });

        return Json(new
        {
            success = false
        });
    }
}
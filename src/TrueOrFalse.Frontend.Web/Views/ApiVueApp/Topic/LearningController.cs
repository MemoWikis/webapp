using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Seedworks.Web.State;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class LearningController : BaseController
{
    private class SessionData
    {
        public SessionData(string currentSessionHeader = "", int currentStepIdx = -1, bool isLastStep = false, int skipStepIdx = -1, int learningSessionId = -1)
        {
            CurrentSessionHeader = currentSessionHeader;
            SkipStepIdx = skipStepIdx;
            IsLastStep = isLastStep;
            LearningSessionId = learningSessionId;
        }

        public string CurrentSessionHeader { get; private set; }
        public int SkipStepIdx { get; private set; }
        public bool IsLastStep { get; private set; }
        public Guid CurrentStepGuid { get; private set; }
        public int LearningSessionId { get; private set; }
    }

    [HttpPost]
    public JsonResult GetCount(LearningSessionConfig config)
    {
        if (config.CurrentUserId == 0 && SessionUser.IsLoggedIn)
            config.CurrentUserId = SessionUser.UserId;

        var learningSession = LearningSessionCreator.BuildLearningSession(config);

        return Json(learningSession.QuestionCounter);
    }

}
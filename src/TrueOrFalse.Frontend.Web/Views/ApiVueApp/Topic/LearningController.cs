using System;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class LearningController : BaseController
{
    private readonly LearningSessionCreator _learningSessionCreator;

    public LearningController(SessionUser sessionUser,LearningSessionCreator learningSessionCreator) : base(sessionUser)
    {
        _learningSessionCreator = learningSessionCreator;
    }
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
        if (config.CurrentUserId == 0 && _sessionUser.IsLoggedIn)
            config.CurrentUserId = _sessionUser.UserId;

        var learningSession = _learningSessionCreator.BuildLearningSession(config);
        
        return Json(learningSession.QuestionCounter);
    }

}
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

    [HttpPost]
    public JsonResult GetCount([FromBody] LearningSessionConfig config)
    {
        if (config.CurrentUserId == 0 && _sessionUser.IsLoggedIn)
            config.CurrentUserId = _sessionUser.UserId;

        return Json(_learningSessionCreator.GetQuestionCounterForLearningSession(config));
    }

}
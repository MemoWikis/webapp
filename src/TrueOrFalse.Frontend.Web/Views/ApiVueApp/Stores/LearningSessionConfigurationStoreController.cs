using System;
using Microsoft.AspNetCore.Mvc;

namespace TrueOrFalse.View.Web.Views.ApiVueApp.Stores;

public class LearningSessionConfigurationStoreController : BaseController
{
    private readonly LearningSessionCreator _learningSessionCreator;

    public LearningSessionConfigurationStoreController(SessionUser sessionUser, LearningSessionCreator learningSessionCreator) : base(sessionUser)
    {
        _learningSessionCreator = learningSessionCreator;
    }

    [HttpPost]
    public QuestionCounter GetCount([FromBody] LearningSessionConfig config)
    {
        if (config.CurrentUserId == 0 && _sessionUser.IsLoggedIn)
            config.CurrentUserId = _sessionUser.UserId;

        return _learningSessionCreator.GetQuestionCounterForLearningSession(config);
    }

}
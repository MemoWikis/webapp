﻿using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class LearningController(
    SessionUser _sessionUser,
    LearningSessionCreator _learningSessionCreator) : Controller
{
    public record struct GetCountResult(
        int InWuwi,
        int NotInWuwi,
        int CreatedByCurrentUser,
        int NotCreatedByCurrentUser,
        int Private,
        int Public,
        int NotLearned,
        int NeedsLearning,
        int NeedsConsolidation,
        int Solid,
        int Max);

    [HttpPost]
    public GetCountResult GetCount([FromBody] LearningSessionConfig config)
    {
        if (config.CurrentUserId == 0 && _sessionUser.IsLoggedIn)
            config.CurrentUserId = _sessionUser.UserId;

        var questionCounter = _learningSessionCreator.GetQuestionCounterForLearningSession(config);

        return new GetCountResult(
            questionCounter.InWuwi,
            questionCounter.NotInWuwi,
            questionCounter.CreatedByCurrentUser,
            questionCounter.NotCreatedByCurrentUser,
            questionCounter.Private,
            questionCounter.Public,
            questionCounter.NotLearned,
            questionCounter.NeedsLearning,
            questionCounter.NeedsConsolidation,
            questionCounter.Solid,
            questionCounter.Max);
    }
}
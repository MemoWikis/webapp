﻿using System;
using System.Web.Mvc;

public class LearningSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/LearningSession/LearningSessionResult.aspx";

    [SetThemeMenu(isLearningSessionPage: true)]
    public ActionResult LearningSessionResult(int learningSessionId, string learningSessionName)
    {
        var learningSession = LearningSessionCache.GetLearningSession();

        if (learningSession.User != SessionUser.User)
            throw new Exception("not logged in or not possessing user");

        return View(_viewLocation, new LearningSessionResultModel(learningSession));
    }
}

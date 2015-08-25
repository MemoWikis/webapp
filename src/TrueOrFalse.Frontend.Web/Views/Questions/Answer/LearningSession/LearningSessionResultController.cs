﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentNHibernate.Utils;

public class LearningSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/LearningSession/LearningSessionResult.aspx";

    public ActionResult LearningSessionResult(int learningSessionId, string setName)
    {
        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        learningSession.Steps
            .Where(s => s.AnswerState == StepAnswerState.Uncompleted)
            .Each( s => LearningSessionStep.Skip(s.Id));

        return View(_viewLocation, new LearningSessionResultModel(learningSession));
    }

}

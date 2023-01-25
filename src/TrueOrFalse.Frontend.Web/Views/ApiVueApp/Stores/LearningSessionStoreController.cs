﻿using System.Linq;
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
        {
            var firstStep = learningSession.Steps.First();
            return Json(new
            {
                success = true,
                steps = learningSession.Steps.Select((s, index) => new
                {
                    id = s.Question.Id,
                    state = s.AnswerState,
                    index = index
                }).ToArray(),
                activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
                firstStep = new
                {
                    state = firstStep.AnswerState,
                    id = firstStep.Question.Id,
                    index = 0
                }

            });
        }

        return Json(new
        {
            success = false
        });
    }

    [HttpGet]
    public JsonResult GetLastStepInQuestionList(int index)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            return Json(new
            {
                success = true,
                steps = learningSession.Steps.Select((s, index) => new
                {
                    id = s.Question.Id,
                    state = s.AnswerState,
                    index = index
                }).ToArray(),
                activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(new
        {
            success = false
        }, JsonRequestBehavior.AllowGet);
    }
}
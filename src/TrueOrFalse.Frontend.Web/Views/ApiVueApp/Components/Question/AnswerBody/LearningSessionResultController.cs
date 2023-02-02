using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TrueOrFalse;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class VueLearningSessionResultController: BaseController
{
    [HttpGet]
    public JsonResult Get()
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        var model = new LearningSessionResultModel(learningSession);
        var steps = model.AnsweredStepsGrouped.Select(g =>
            new {
            id = g.First().Question.Id,
            count = g.Count(),
            states = g.Select(s => s.AnswerState).ToArray(),
            }).ToArray();

        return Json(new
        {
            uniqueQuestionCount = model.NumberUniqueQuestions,
            correct = new
            {
                percentage = model.NumberCorrectPercentage,
                count = model.NumberCorrectAnswers
            },
            correctAfterRepetition = new
            {
                percentage = model.NumberCorrectAfterRepetitionPercentage,
                count = model.NumberCorrectAfterRepetitionAnswers
            },
            wrong = new
            {
                percentage = model.NumberWrongAnswersPercentage,
                count = model.NumberWrongAnswers
            },
            notAnswered = new
            {
                percentage = model.NumberNotAnsweredPercentage,
                count = model.NumberNotAnswered
            },
            steps = steps
        }, JsonRequestBehavior.AllowGet);
    }
}
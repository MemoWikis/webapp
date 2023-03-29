using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;
public class DeleteQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public DeleteQuestionController(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    [HttpPost]
    public JsonResult DeleteDetails(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        var canBeDeleted = QuestionDelete.CanBeDeleted(question.Creator.Id, question);

        return Json(new
        {
            questionTitle = question.Text.TruncateAtWord(90),
            totalAnswers = question.TotalAnswers(),
            canNotBeDeleted = !canBeDeleted.Yes,
            wuwiCount = canBeDeleted.WuwiCount,
            hasRights = canBeDeleted.HasRights
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult Delete(int questionId, int sessionIndex)
    {
        QuestionDelete.Run(questionId);
        LearningSessionCache.RemoveQuestionFromLearningSession(sessionIndex, questionId);
        return Json(new
        {
            sessionIndex,
            questionId
        });
    }
}
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class TopicLearningQuestionListController: BaseController
{
    [HttpPost]
    public JsonResult LoadQuestions(int itemCountPerPage, int pageNumber)
    {
        return Json(QuestionListModel.PopulateQuestionsOnPage(pageNumber, itemCountPerPage));
    }
}
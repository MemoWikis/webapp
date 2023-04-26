using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;
public class QuestionEditModalController : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public QuestionEditModalController(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Create(QuestionEditModalControllerLogic.QuestionDataJson questionDataJson)
    {
        var data = new QuestionEditModalControllerLogic(_questionRepo).Create(questionDataJson); 
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Edit(QuestionEditModalControllerLogic.QuestionDataJson questionDataJson)
    {
        var data = new QuestionEditModalControllerLogic(_questionRepo).Edit(questionDataJson);
        return Json(data, JsonRequestBehavior.AllowGet);
    }


    [HttpGet]
    public JsonResult GetData(int id)
    {
        var data = new QuestionEditModalControllerLogic(_questionRepo).GetData(id);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache().Count;
}

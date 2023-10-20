using HelperClassesControllers;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;
public class QuestionEditModalController : BaseController
{
    private readonly QuestionEditModalControllerLogic _questionEditModalControllerLogic;

    public QuestionEditModalController(SessionUser sessionUser,
        QuestionEditModalControllerLogic questionEditModalControllerLogic) 
        : base(sessionUser)
    {
        _questionEditModalControllerLogic = questionEditModalControllerLogic;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Create([FromBody] QuestionDataJson questionDataJson)
    {
        var data = _questionEditModalControllerLogic
            .Create(questionDataJson); 

        return Json(data);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Edit([FromBody] QuestionDataJson questionDataJson)
    {
        var data = _questionEditModalControllerLogic.Edit(questionDataJson);
        return Json(data);
    }


    [HttpGet]
    public JsonResult GetData(int id)
    {
        var data = _questionEditModalControllerLogic.GetData(id);
        return Json(data);
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;
}

﻿using System.Web.Mvc;

namespace VueApp;
public class QuestionEditModalController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;

    public QuestionEditModalController(QuestionRepo questionRepo,
        SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge) :base(sessionUser)
    {
        _questionRepo = questionRepo;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Create(QuestionEditModalControllerLogic.QuestionDataJson questionDataJson)
    {
        var data = new QuestionEditModalControllerLogic(_questionRepo,_sessionUser,_learningSessionCache,_permissionCheck,_learningSessionCreator,_questionInKnowledge).Create(questionDataJson); 
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Edit(QuestionEditModalControllerLogic.QuestionDataJson questionDataJson)
    {
        var data = new QuestionEditModalControllerLogic(_questionRepo, _sessionUser, _learningSessionCache, _permissionCheck, _learningSessionCreator, _questionInKnowledge).Edit(questionDataJson);
        return Json(data, JsonRequestBehavior.AllowGet);
    }


    [HttpGet]
    public JsonResult GetData(int id)
    {
        var data = new QuestionEditModalControllerLogic(_questionRepo, _sessionUser, _learningSessionCache, _permissionCheck, _learningSessionCreator, _questionInKnowledge).GetData(id);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;
}

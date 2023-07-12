using System.Web.Mvc;

namespace VueApp;
public class QuestionEditModalController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly QuestionEditModalControllerLogic _questionEditModalControllerLogic;

    public QuestionEditModalController(QuestionRepo questionRepo,
        SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge, 
        CategoryValuationRepo categoryValuationRepo, 
        CategoryRepository categoryRepository,
        ImageMetaDataRepo imageMetaDataRepo,
        QuestionEditModalControllerLogic questionEditModalControllerLogic) :base(sessionUser)
    {
        _questionRepo = questionRepo;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _categoryValuationRepo = categoryValuationRepo;
        _categoryRepository = categoryRepository;
        _imageMetaDataRepo = imageMetaDataRepo;
        _questionEditModalControllerLogic = questionEditModalControllerLogic;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Create(QuestionEditModalControllerLogic.QuestionDataJson questionDataJson)
    {
        var data = _questionEditModalControllerLogic
            .Create(questionDataJson); 

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Edit(QuestionEditModalControllerLogic.QuestionDataJson questionDataJson)
    {
        var data = _questionEditModalControllerLogic.Edit(questionDataJson);
        return Json(data, JsonRequestBehavior.AllowGet);
    }


    [HttpGet]
    public JsonResult GetData(int id)
    {
        var data = _questionEditModalControllerLogic.GetData(id);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;
}

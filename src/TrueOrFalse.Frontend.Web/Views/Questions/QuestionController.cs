
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;


public class QuestionController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private IActionContextAccessor _actionAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public QuestionController(SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IActionContextAccessor actionAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _actionAccessor = actionAccessor;
        _questionReadingRepo = questionReadingRepo;
    }
    public JsonResult LoadQuestion(int questionId)
    {
        var user = _sessionUser;
        var userQuestionValuation = _sessionUserCache.GetItem(user.UserId).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;

        var links = new Links(_actionAccessor, _httpContextAccessor);
        question.LinkToQuestion = links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                _httpContextAccessor, 
                _webHostEnvironment,
                _questionReadingRepo)
            .GetImageUrl(40, true)
            .Url;
        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation.ContainsKey(q.Id) && user != null)
        {
            question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return Json(question);
    }
}
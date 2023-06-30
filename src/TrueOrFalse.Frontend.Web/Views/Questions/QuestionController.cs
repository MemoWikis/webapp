using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;


public class QuestionController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;

    public QuestionController(SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        CategoryValuationRepo categoryValuationRepo,
        ImageMetaDataRepo imageMetaDataRepo)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _categoryValuationRepo = categoryValuationRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
    }
    public JsonResult LoadQuestion(int questionId)
    {
        var user = _sessionUser;
        var userQuestionValuation = SessionUserCache.GetItem(user.UserId, _categoryValuationRepo).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
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
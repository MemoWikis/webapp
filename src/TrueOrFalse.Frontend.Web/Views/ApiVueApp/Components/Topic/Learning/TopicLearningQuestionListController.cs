using System.Collections.Concurrent;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class TopicLearningQuestionListController: Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;

    public TopicLearningQuestionListController(SessionUser sessionUser,
        LearningSessionCreator learningSessionCreator,
        LearningSessionCache learningSessionCache,
        CategoryValuationReadingRepo categoryValuationReadingRepo, 
        ImageMetaDataRepo imageMetaDataRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo) 
    {
        _sessionUser = sessionUser;
        _learningSessionCreator = learningSessionCreator;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
    }
    [HttpPost]
    public JsonResult LoadQuestions(int itemCountPerPage, int pageNumber, int topicId)
    {
        if (_learningSessionCache.GetLearningSession() == null || topicId != _learningSessionCache.GetLearningSession().Config.CategoryId)
        {
            var config = new LearningSessionConfig
            {
                CategoryId = topicId,
                CurrentUserId = _sessionUser.IsLoggedIn ? _sessionUser.UserId : default
            };
            _learningSessionCache.AddOrUpdate(_learningSessionCreator.BuildLearningSession(config));
        }

        return Json(new QuestionListModel(_learningSessionCache,_sessionUser, _categoryValuationReadingRepo, _imageMetaDataRepo, _userReadingRepo, _questionValuationRepo)
            .PopulateQuestionsOnPage(pageNumber, itemCountPerPage));
    }

    [HttpGet]
    public JsonResult LoadNewQuestion(int index)
    {
        var steps = _learningSessionCache.GetLearningSession().Steps;
        var question = steps[index].Question;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;

        return Json( new {
            Id = question.Id,
            Title = question.Text,
            LinkToQuestion = Links.GetUrl(question),
            ImageData = new ImageFrontendData(_imageMetaDataRepo.GetBy(question.Id, ImageType.Question)).GetImageUrl(40, true).Url,
            LearningSessionStepCount = steps.Count,
            LinkToQuestionVersions = Links.QuestionHistory(question.Id),
            LinkToComment = Links.GetUrl(question) + "#JumpLabel",
            CorrectnessProbability = hasUserValuation ? userQuestionValuation[question.Id].CorrectnessProbability : question.CorrectnessProbability,
            KnowledgeStatus = hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            Visibility = question.Visibility,
            SessionIndex = index,
            IsInWishknowledge = hasUserValuation && userQuestionValuation[question.Id].IsInWishKnowledge,
            HasPersonalAnswer = false
        }, JsonRequestBehavior.AllowGet);
    }
}
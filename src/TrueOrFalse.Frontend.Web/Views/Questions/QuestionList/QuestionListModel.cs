using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel
{
    private SessionUser _sessionUser { get; }
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public int CategoryId;

    public QuestionListModel(LearningSessionCache learningSessionCache,
        SessionUser sessionUser,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        SessionUserCache sessionUserCache,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _sessionUserCache = sessionUserCache;
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public List<QuestionListJson.Question> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage)
    {
        var learningSession = _learningSessionCache.GetLearningSession();

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var steps = learningSession.Steps;
        var stepsOfCurrentPage = steps.Skip(itemCountPerPage * (currentPage - 1)).Take(itemCountPerPage).ToList();
        stepsOfCurrentPage.RemoveAll(s => s.Question.Id == 0);

        var newQuestionList = new List<QuestionListJson.Question>();

        foreach (var step in stepsOfCurrentPage)
        {
            var q = step.Question;

            var hasUserValuation = userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn;
            var links = new Links(_actionContextAccessor, _httpContextAccessor);
            var question = new QuestionListJson.Question
            {
                Id = q.Id,
                Title = q.Text,
                LinkToQuestion = links.GetUrl(q),
                ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),_httpContextAccessor, _webHostEnvironment)
                    .GetImageUrl(40, true)
                    .Url,
                LearningSessionStepCount = steps.Count,
                LinkToQuestionVersions = links.QuestionHistory(q.Id),
                LinkToComment = links.GetUrl(q) + "#JumpLabel",
                CorrectnessProbability = q.CorrectnessProbability,
                Visibility = q.Visibility,
                SessionIndex = steps.IndexOf(step),
                KnowledgeStatus = hasUserValuation ? userQuestionValuation[q.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            };

            if (userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn)
            {
                question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
                question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }
            newQuestionList.Add(question);
        }

        return newQuestionList;
    }
}
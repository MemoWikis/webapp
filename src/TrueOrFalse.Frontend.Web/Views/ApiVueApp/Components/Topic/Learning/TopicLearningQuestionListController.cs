using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

public class TopicLearningQuestionListController(
    SessionUser _sessionUser,
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache,
    CategoryValuationReadingRepo _categoryValuationReadingRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    UserReadingRepo _userReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    SessionUserCache _sessionUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    public readonly record struct LoadQuestionsJson(
        int ItemCountPerPage,
        int PageNumber,
        int TopicId);

    [HttpPost]
    public List<QuestionListJson.Question> LoadQuestions([FromBody] LoadQuestionsJson json)
    {
        if (_learningSessionCache.GetLearningSession() == null || json.TopicId !=
            _learningSessionCache.GetLearningSession()?.Config.CategoryId)
            _learningSessionCreator.LoadDefaultSessionIntoCache(json.TopicId, _sessionUser.UserId);

        return new QuestionListModel(
                _learningSessionCache,
                _sessionUser,
                _categoryValuationReadingRepo,
                _imageMetaDataReadingRepo,
                _userReadingRepo,
                _questionValuationReadingRepo,
                _sessionUserCache,
                _actionContextAccessor,
                _httpContextAccessor,
                _webHostEnvironment,
                _questionReadingRepo)
            .PopulateQuestionsOnPage(json.PageNumber, json.ItemCountPerPage);
    }

    public readonly record struct LoadNewQuestionJson(
        bool Success,
        QuestionListJson.Question Data,
        string MessageKey);

    [HttpGet]
    public LoadNewQuestionJson LoadNewQuestion([FromRoute] int id)
    {
        var index = id;
        var session = _learningSessionCache.GetLearningSession();
        if (session == null)
            return new LoadNewQuestionJson
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };
        var steps = session.Steps;

        var question = steps[index].Question;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation != null &&
                               userQuestionValuation.ContainsKey(question.Id) &&
                               _sessionUser.IsLoggedIn;

        return new LoadNewQuestionJson
        {
            Success = true,
            Data = new QuestionListJson.Question
            {
                Id = question.Id,
                Title = question.Text,
                LinkToQuestion =
                    new Links(_actionContextAccessor, _httpContextAccessor).GetUrl(question),
                ImageData = new ImageFrontendData(
                        _imageMetaDataReadingRepo.GetBy(question.Id, ImageType.Question),
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetImageUrl(40, true)
                    .Url,
                LearningSessionStepCount = steps.Count,
                LinkToQuestionVersions =
                    new Links(_actionContextAccessor, _httpContextAccessor).QuestionHistory(
                        question.Id),
                LinkToComment =
                    new Links(_actionContextAccessor, _httpContextAccessor).GetUrl(question) +
                    "#JumpLabel",
                CorrectnessProbability = hasUserValuation
                    ? userQuestionValuation[question.Id].CorrectnessProbability
                    : question.CorrectnessProbability,
                KnowledgeStatus = hasUserValuation
                    ? userQuestionValuation[question.Id].KnowledgeStatus
                    : KnowledgeStatus.NotLearned,
                Visibility = question.Visibility,
                SessionIndex = index,
                IsInWishknowledge = hasUserValuation &&
                                    userQuestionValuation[question.Id].IsInWishKnowledge,
                HasPersonalAnswer = false
            }
        };
    }
}
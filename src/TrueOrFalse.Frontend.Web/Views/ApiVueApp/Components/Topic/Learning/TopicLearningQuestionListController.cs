using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

public class TopicLearningQuestionListController(
    SessionUser _sessionUser,
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck) : Controller
{
    public record struct LoadQuestionsResult(
        int Id,
        string Title,
        int CorrectnessProbability,
        string LinkToQuestion,
        string ImageData,
        bool IsInWishknowledge,
        bool HasPersonalAnswer,
        int LearningSessionStepCount,
        string LinkToComment,
        string LinkToQuestionVersions,
        int SessionIndex,
        QuestionVisibility Visibility,
        int CreatorId = 0,
        KnowledgeStatus KnowledgeStatus = KnowledgeStatus.NotLearned
    );

    public readonly record struct LoadQuestionsJson(
        int ItemCountPerPage,
        int PageNumber,
        int TopicId);

    [HttpPost]
    public List<LoadQuestionsResult> LoadQuestions([FromBody] LoadQuestionsJson json)
    {
        if (_permissionCheck.CanViewCategory(json.TopicId))
        {
            if (_learningSessionCache.GetLearningSession() == null ||
                json.TopicId != _learningSessionCache.GetLearningSession()?.Config.CategoryId)
                _learningSessionCreator.LoadDefaultSessionIntoCache(json.TopicId,
                    _sessionUser.UserId);

            return PopulateQuestionsOnPage(json.PageNumber, json.ItemCountPerPage);
        }

        return new List<LoadQuestionsResult>();
    }

    private List<LoadQuestionsResult> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage)
    {
        var learningSession = _learningSessionCache.GetLearningSession();

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var steps = learningSession.Steps;
        var stepsOfCurrentPage = steps.Skip(itemCountPerPage * (currentPage - 1))
            .Take(itemCountPerPage).ToList();
        stepsOfCurrentPage.RemoveAll(s => s.Question.Id == 0);

        var newQuestionList = new List<LoadQuestionsResult>();

        foreach (var step in stepsOfCurrentPage)
        {
            var q = step.Question;

            var hasUserValuation =
                userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn;
            var links = new Links(_actionContextAccessor, _httpContextAccessor);
            var question = new LoadQuestionsResult
            {
                Id = q.Id,
                Title = q.Text,
                LinkToQuestion = links.GetUrl(q),
                ImageData = new ImageFrontendData(
                        _imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetImageUrl(40, true)
                    .Url,
                LearningSessionStepCount = steps.Count,
                LinkToQuestionVersions = links.QuestionHistory(q.Id),
                LinkToComment = links.GetUrl(q) + "#JumpLabel",
                CorrectnessProbability = q.CorrectnessProbability,
                Visibility = q.Visibility,
                SessionIndex = steps.IndexOf(step),
                KnowledgeStatus = hasUserValuation
                    ? userQuestionValuation[q.Id].KnowledgeStatus
                    : KnowledgeStatus.NotLearned,
            };

            if (userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn)
            {
                question.CorrectnessProbability =
                    userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
                question.HasPersonalAnswer =
                    userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }

            newQuestionList.Add(question);
        }

        return newQuestionList;
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
            ? _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
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
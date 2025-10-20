using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Concurrent;

public class PageLearningQuestionListController(
    SessionUser _sessionUser,
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck) : ApiBaseController
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
        int PageId);

    [HttpPost]
    public List<LoadQuestionsResult> LoadQuestions([FromBody] LoadQuestionsJson json)
    {
        if (_permissionCheck.CanViewPage(json.PageId))
        {
            var learningSession = _learningSessionCache.GetLearningSession(log: false);
            if (learningSession == null || json.PageId != learningSession.Config.PageId)
            {
                learningSession = _learningSessionCreator.LoadDefaultSessionIntoCache(
                    json.PageId,
                    _sessionUser.UserId);
            }

            return PopulateQuestionsOnPage(json.PageNumber, json.ItemCountPerPage, learningSession);
        }

        return new List<LoadQuestionsResult>();
    }

    private List<LoadQuestionsResult> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage, LearningSession learningSession)
    {
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var steps = learningSession.Steps;
        var stepsOfCurrentPage = steps
            .Skip(itemCountPerPage * (currentPage - 1))
            .Take(itemCountPerPage)
            .ToList();

        // Ensure no placeholder or invalid questions are shown
        stepsOfCurrentPage.RemoveAll(s => s.Question.Id == 0);

        var newQuestionList = new List<LoadQuestionsResult>();
        var links = new Links(_actionContextAccessor, _httpContextAccessor);

        foreach (var step in stepsOfCurrentPage)
        {
            var q = step.Question;
            var hasUserValuation = userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn;

            var question = new LoadQuestionsResult
            {
                Id = q.Id,
                Title = q.Text,
                LinkToQuestion = links.GetUrl(q),
                ImageData = new ImageFrontendData(
                        _imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                        _httpContextAccessor,
                        _questionReadingRepo
                    )
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
                    : KnowledgeStatus.NotLearned
            };

            if (hasUserValuation)
            {
                question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishknowledge;
                question.HasPersonalAnswer =
                    userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }

            newQuestionList.Add(question);
        }

        return newQuestionList;
    }

    public readonly record struct LoadNewQuestionResponse(
        bool Success,
        QuestionListJson.Question Data,
        [CanBeNull] string MessageKey);

    // Re-use helper to avoid duplicate code
    private QuestionListJson.Question BuildLoadNewQuestionData(
        QuestionCacheItem question,
        int sessionIndex,
        int totalSteps,
        ConcurrentDictionary<int, QuestionValuationCacheItem> userQuestionValuation)
    {
        var links = new Links(_actionContextAccessor, _httpContextAccessor);

        var hasUserValuation = userQuestionValuation != null &&
                               userQuestionValuation.ContainsKey(question.Id) &&
                               _sessionUser.IsLoggedIn;

        return new QuestionListJson.Question
        {
            Id = question.Id,
            Title = question.Text,
            LinkToQuestion = links.GetUrl(question),
            ImageData = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(question.Id, ImageType.Question),
                    _httpContextAccessor,
                    _questionReadingRepo
                ).GetImageUrl(40, true).Url,
            LearningSessionStepCount = totalSteps,
            LinkToQuestionVersions = links.QuestionHistory(question.Id),
            LinkToComment = links.GetUrl(question) + "#JumpLabel",
            CorrectnessProbability = hasUserValuation
                    ? userQuestionValuation[question.Id].CorrectnessProbability
                    : question.CorrectnessProbability,
            KnowledgeStatus = hasUserValuation
                    ? userQuestionValuation[question.Id].KnowledgeStatus
                    : KnowledgeStatus.NotLearned,
            Visibility = question.Visibility,
            SessionIndex = sessionIndex,
            IsInWishknowledge = hasUserValuation &&
                                    userQuestionValuation[question.Id].IsInWishknowledge,
            HasPersonalAnswer = false // Or replicate any personal-answer logic if needed
        };
    }

    [HttpGet]
    public LoadNewQuestionResponse LoadNewQuestion([FromRoute] int id)
    {
        var session = _learningSessionCache.GetLearningSession();
        if (session == null)
        {
            return new LoadNewQuestionResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };
        }

        var steps = session.Steps;
        // Guard against invalid index
        if (id < 0 || id >= steps.Count)
        {
            return new LoadNewQuestionResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };
        }

        var question = steps[id].Question;
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        // Build and return the response via our helper

        return new LoadNewQuestionResponse
        {
            Success = true,
            Data = BuildLoadNewQuestionData(question, id, steps.Count, userQuestionValuation)
        };
    }

    public record struct LoadNewQuestionsRequest(
        int StartIndex,
        int EndIndex);

    public readonly record struct LoadNewQuestionsResponse(
        bool Success,
        [CanBeNull] List<QuestionListJson.Question> Data = null,
        [CanBeNull] string MessageKey = null);

    [HttpPost]
    public LoadNewQuestionsResponse LoadNewQuestions([FromBody] LoadNewQuestionsRequest request)
    {
        var session = _learningSessionCache.GetLearningSession();
        if (session == null)
        {
            return new LoadNewQuestionsResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };
        }

        var steps = session.Steps;

        // Normalize the requested indices
        var start = request.StartIndex < 0 ? 0 : request.StartIndex;
        var end = request.EndIndex >= steps.Count
            ? steps.Count - 1
            : request.EndIndex;

        // If after adjusting, our range is invalid, return nothing
        if (start > end)
            return new LoadNewQuestionsResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var questions = new List<QuestionListJson.Question>();
        for (int i = start; i <= end; i++)
        {
            var question = steps[i].Question;
            questions.Add(BuildLoadNewQuestionData(
                question,
                i,
                steps.Count,
                userQuestionValuation)
            );
        }

        return new LoadNewQuestionsResponse
        {
            Success = true,
            Data = questions
        };
    }
}

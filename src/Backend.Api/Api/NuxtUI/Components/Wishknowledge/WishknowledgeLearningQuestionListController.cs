using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Concurrent;

public class WishknowledgeLearningQuestionListController(
    SessionUser _sessionUser,
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck) : ApiBaseController
{
    public record struct LoadQuestionsResult(
        int Id,
        string Title,
        int CorrectnessProbability,
        string ImageData,
        bool IsInWishKnowledge,
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
        int PageNumber);

    [HttpPost]
    public List<LoadQuestionsResult> LoadQuestions([FromBody] LoadQuestionsJson json)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new List<LoadQuestionsResult>();
        }

        var learningSession = _learningSessionCache.GetLearningSession(log: false);
        if (learningSession == null || learningSession.Config.PageId != 0)
        {
            learningSession = _learningSessionCreator.LoadDefaultSessionIntoCache(
                0, // PageId = 0 for wishknowledge
                _sessionUser.UserId);
        }

        if (learningSession?.Steps == null || learningSession.Steps.Count == 0)
        {
            return new List<LoadQuestionsResult>();
        }

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var steps = learningSession.Steps;
        var itemsToSkip = (json.PageNumber - 1) * json.ItemCountPerPage;
        var itemsToTake = json.ItemCountPerPage;

        return steps
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .Where(step => _permissionCheck.CanView(step.Question))
            .Select((step, index) =>
            {
                var question = step.Question;
                var sessionIndex = itemsToSkip + index;

                var hasUserValuation = userQuestionValuation != null &&
                                     userQuestionValuation.ContainsKey(question.Id) &&
                                     _sessionUser.IsLoggedIn;

                return new LoadQuestionsResult(
                    Id: question.Id,
                    Title: question.Text,
                    CorrectnessProbability: hasUserValuation
                        ? userQuestionValuation[question.Id].CorrectnessProbability
                        : question.CorrectnessProbability,
                    ImageData: new ImageFrontendData(
                            _imageMetaDataReadingRepo.GetBy(question.Id, ImageType.Question),
                            _httpContextAccessor,
                            _questionReadingRepo
                        ).GetImageUrl(40, true).Url,
                    IsInWishKnowledge: hasUserValuation &&
                                     userQuestionValuation[question.Id].IsInWishKnowledge,
                    HasPersonalAnswer: hasUserValuation,
                    LearningSessionStepCount: steps.Count,
                    LinkToComment: "",
                    LinkToQuestionVersions: "",
                    SessionIndex: sessionIndex,
                    Visibility: question.Visibility,
                    CreatorId: question.Creator?.Id ?? 0,
                    KnowledgeStatus: hasUserValuation
                        ? userQuestionValuation[question.Id].KnowledgeStatus
                        : KnowledgeStatus.NotLearned
                );
            })
            .ToList();
    }

    public record struct LoadNewQuestionResponse(
        bool Success,
        LoadQuestionsResult Data,
        [CanBeNull] string MessageKey);

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

        var hasUserValuation = userQuestionValuation != null &&
                             userQuestionValuation.ContainsKey(question.Id) &&
                             _sessionUser.IsLoggedIn;

        var data = new LoadQuestionsResult(
            Id: question.Id,
            Title: question.Text,
            CorrectnessProbability: hasUserValuation
                ? userQuestionValuation[question.Id].CorrectnessProbability
                : question.CorrectnessProbability,
            ImageData: new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(question.Id, ImageType.Question),
                    _httpContextAccessor,
                    _questionReadingRepo
                ).GetImageUrl(40, true).Url,
            IsInWishKnowledge: hasUserValuation &&
                             userQuestionValuation[question.Id].IsInWishKnowledge,
            HasPersonalAnswer: hasUserValuation,
            LearningSessionStepCount: steps.Count,
            LinkToComment: "",
            LinkToQuestionVersions: "",
            SessionIndex: id,
            Visibility: question.Visibility,
            CreatorId: question.Creator?.Id ?? 0,
            KnowledgeStatus: hasUserValuation
                ? userQuestionValuation[question.Id].KnowledgeStatus
                : KnowledgeStatus.NotLearned
        );

        return new LoadNewQuestionResponse
        {
            Success = true,
            Data = data
        };
    }
}
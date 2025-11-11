// Refactored Learning Session Creator with better separation of concerns
using System.Collections.Concurrent;

public class LearningSessionCreator(
    SessionUser _sessionUser,
    LearningSessionCache _learningSessionCache,
    PermissionCheck _permissionCheck,
    ExtendedUserCache _extendedUserCache,
    QuestionFilterService _questionFilterService,
    QuestionSortingService _questionSortingService,
    QuestionCounterService _questionCounterService,
    LearningSessionResultService _resultService)
    : IRegisterAsInstancePerLifetime
{
    public LearningSessionResultStep GetLearningSessionResult(LearningSessionConfig config)
    {
        var learningSession = GetLearningSession(config);
        var result = new LearningSessionResultStep();

        if (learningSession.Steps.Any())
        {
            result = _resultService.FillLearningSessionResult(learningSession, result);
        }

        return result;
    }

    public LearningSessionResultStep GetLearningSessionResult(
        LearningSessionConfig config,
        int questionId)
    {
        var result = new LearningSessionResultStep();

        // Validate question access
        var (isValid, messageKey) = _resultService.ValidateQuestionAccess(config.PageId, questionId);
        if (!isValid)
        {
            result.MessageKey = messageKey;
            return result;
        }

        // Get all questions for the page
        var allQuestions = GetAllQuestionsForPage(config.PageId);
        var learningSession = GetLearningSession(config, questionId, allQuestions);

        if (!learningSession.Steps.Any())
        {
            return result;
        }

        // Load specific question or set appropriate message
        if (learningSession.Steps.Any(s => s.Question.Id == questionId))
        {
            var questionIndex = learningSession.Steps.FindIndex(s => s.Question.Id == questionId);
            learningSession.LoadSpecificQuestion(questionIndex);
        }
        else if (result.MessageKey == null)
        {
            result.MessageKey = FrontendMessageKeys.Info.Question.NotInFilter;
        }

        result = _resultService.FillLearningSessionResult(learningSession, result);
        return result;
    }

    public LearningSessionResultStep GetStep(int index)
    {
        var result = new LearningSessionResultStep();
        var learningSession = _learningSessionCache.GetLearningSession();
        learningSession?.LoadSpecificQuestion(index);

        return _resultService.FillLearningSessionResult(learningSession, result);
    }

    public LearningSession LoadDefaultSessionIntoCache(int pageId, int userId = 0)
    {
        var config = new LearningSessionConfig
        {
            PageId = pageId,
            CurrentUserId = userId
        };
        var learningSession = BuildLearningSession(config);
        _learningSessionCache.AddOrUpdate(learningSession);
        return _learningSessionCache.GetLearningSession()!;
    }

    public LearningSession GetLearningSession(LearningSessionConfig config)
    {
        var learningSession = BuildLearningSession(config);
        _learningSessionCache.AddOrUpdate(learningSession);
        return _learningSessionCache.GetLearningSession()!;
    }

    public LearningSession GetLearningSession(
        LearningSessionConfig config,
        int questionId,
        IList<QuestionCacheItem> allQuestions)
    {
        var learningSession = BuildLearningSession(config, allQuestions, questionId);
        _learningSessionCache.AddOrUpdate(learningSession);
        return _learningSessionCache.GetLearningSession()!;
    }

    public QuestionCounter GetQuestionCounterForLearningSession(LearningSessionConfig config)
    {
        return BuildLearningSession(config).QuestionCounter;
    }

    public void InsertNewQuestionToLearningSession(
        QuestionCacheItem question,
        int sessionIndex,
        LearningSessionConfig config)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession == null)
        {
            return;
        }

        var step = new LearningSessionStep(question);
        var allQuestionValuations = _extendedUserCache.GetQuestionValuations(config.CurrentUserId);
        var userQuestionValuations = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var questionProperties = _questionFilterService.BuildQuestionProperties(
            question,
            config,
            allQuestionValuations,
            userQuestionValuations
        );

        _questionCounterService.Count(questionProperties, learningSession.QuestionCounter);

        if (questionProperties.AddToLearningSession)
        {
            if (learningSession.Steps.Count > sessionIndex + 1)
            {
                learningSession.Steps.Insert(sessionIndex + 1, step);
            }
            else
            {
                learningSession.Steps.Add(step);
            }
        }

        learningSession.QuestionCounter.Max += 1;
        _learningSessionCache.AddOrUpdate(learningSession);
    }

    public LearningSession BuildLearningSession(LearningSessionConfig config)
    {
        var allQuestions = config.PageId > 0 ? GetAllQuestionsForPage(config.PageId) : GetAllWishknowledgeQuestionsFromUser(_sessionUser.UserId);

        return BuildLearningSession(config, allQuestions);
    }

    public LearningSession BuildLearningSession(
        LearningSessionConfig config,
        IList<QuestionCacheItem> allQuestions,
        int questionId = 0)
    {
        _learningSessionCache.TryRemove();

        // Ensure current user ID is set correctly
        if (_sessionUser.IsLoggedIn && config.CurrentUserId != _sessionUser.UserId)
        {
            config.CurrentUserId = _sessionUser.UserId;
        }

        // Filter questions based on configuration
        var filteredQuestions = _questionFilterService.FilterQuestions(allQuestions, config, _sessionUser.UserId);

        // Build question counter
        var questionCounter = _questionCounterService.BuildCounter(
            allQuestions,
            config,
            _questionFilterService);

        // Build knowledge summary details for sorting
        var knowledgeSummaryDetails = BuildKnowledgeSummaryDetails(
            filteredQuestions,
            config);

        // Handle specific question insertion
        if (questionId > 0 && filteredQuestions.Any(q => q.Id == questionId))
        {
            return BuildLearningSessionWithSpecificQuestion(
                filteredQuestions,
                config,
                questionId,
                knowledgeSummaryDetails,
                questionCounter);
        }

        // Normal session building
        return BuildStandardLearningSession(
            filteredQuestions,
            config,
            knowledgeSummaryDetails,
            questionCounter);
    }

    private LearningSession BuildStandardLearningSession(
        IList<QuestionCacheItem> filteredQuestions,
        LearningSessionConfig config,
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails,
        QuestionCounter questionCounter)
    {
        // Shuffle, limit, and sort questions
        var processedQuestions = _questionSortingService.ShuffleQuestions(filteredQuestions);
        processedQuestions = _questionSortingService.LimitQuestionCount(processedQuestions, config);
        processedQuestions = _questionSortingService.SortQuestions(
            processedQuestions,
            config,
            knowledgeSummaryDetails);

        var steps = processedQuestions
            .Distinct()
            .Select(q => new LearningSessionStep(q))
            .ToList();

        return new LearningSession(steps, config)
        {
            QuestionCounter = questionCounter
        };
    }

    private LearningSession BuildLearningSessionWithSpecificQuestion(
        IList<QuestionCacheItem> filteredQuestions,
        LearningSessionConfig config,
        int questionId,
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails,
        QuestionCounter questionCounter)
    {
        // Remove the specific question from the list temporarily
        var questionsWithoutSpecific = filteredQuestions.Where(q => q.Id != questionId).ToList();

        // Shuffle and limit (leaving space for the specific question)
        var shuffledQuestions = _questionSortingService.ShuffleQuestions(questionsWithoutSpecific);
        var limitedQuestions = LimitQuestionsForSpecificQuestion(shuffledQuestions, config);

        // Insert the specific question at a random position
        var specificQuestion = EntityCache.GetQuestion(questionId);
        var finalQuestions = InsertSpecificQuestionRandomly(limitedQuestions, specificQuestion);

        // Sort the final list
        finalQuestions = _questionSortingService.SortQuestions(
            finalQuestions,
            config,
            knowledgeSummaryDetails);

        var steps = finalQuestions
            .Distinct()
            .Select(q => new LearningSessionStep(q))
            .ToList();

        return new LearningSession(steps, config)
        {
            QuestionCounter = questionCounter
        };
    }


    private IList<QuestionCacheItem> GetAllQuestionsForPage(int pageId)
    {
        return EntityCache.GetPage(pageId)
            .GetAggregatedQuestions(_sessionUser.UserId, permissionCheck: _permissionCheck)
            .Where(questionId => questionId.Id > 0)
            .Where(_permissionCheck.CanView)
            .ToList();
    }

    private IList<QuestionCacheItem> GetAllWishknowledgeQuestionsFromUser(int userId)
    {
        var user = _extendedUserCache.GetUser(userId);
        var wishknowledgeQuestionIds = user.QuestionValuations
            .Where(questionValuation => questionValuation.Value.IsInWishKnowledge)
            .Select(questionValuation => questionValuation.Key)
            .ToList();

        var questions = EntityCache.GetQuestionsByIds(wishknowledgeQuestionIds);

        return questions;
    }

    private IList<KnowledgeSummaryDetail> BuildKnowledgeSummaryDetails(
        IList<QuestionCacheItem> questions,
        LearningSessionConfig config)
    {
        var knowledgeSummaryDetails = new List<KnowledgeSummaryDetail>();

        if (!_sessionUser.IsLoggedIn)
        {
            return knowledgeSummaryDetails;
        }

        var allQuestionValuations = _extendedUserCache.GetQuestionValuations(_sessionUser.UserId);
        var userQuestionValuations = _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations;

        foreach (var question in questions)
        {
            var questionProperties = _questionFilterService.BuildQuestionProperties(
                question,
                config,
                allQuestionValuations,
                userQuestionValuations);

            knowledgeSummaryDetails.Add(new KnowledgeSummaryDetail
            {
                PersonalCorrectnessProbability = questionProperties.PersonalCorrectnessProbability,
                QuestionId = question.Id
            });
        }

        return knowledgeSummaryDetails;
    }

    private static IList<QuestionCacheItem> LimitQuestionsForSpecificQuestion(
        IList<QuestionCacheItem> questions,
        LearningSessionConfig config)
    {
        var maxCount = config.MaxQuestionCount - 1; // Leave space for specific question

        if (maxCount > questions.Count || maxCount <= 0)
        {
            return questions;
        }

        return questions.Take(maxCount).ToList();
    }

    private static IList<QuestionCacheItem> InsertSpecificQuestionRandomly(
        IList<QuestionCacheItem> questions,
        QuestionCacheItem specificQuestion)
    {
        var random = new Random();
        var randomIndex = random.Next(questions.Count + 1);

        var result = questions.ToList();
        result.Insert(randomIndex, specificQuestion);

        return result;
    }
}

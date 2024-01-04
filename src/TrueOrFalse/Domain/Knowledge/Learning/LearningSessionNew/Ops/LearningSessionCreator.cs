using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
public class LearningSessionCreator : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUserCache _sessionUserCache;

    public struct QuestionProperties
    {
        public bool NotLearned;
        public bool NeedsLearning;
        public bool NeedsConsolidation;
        public bool Solid;
        public bool InWuwi;
        public bool NotInWuwi;
        public bool CreatedByCurrentUser;
        public bool NotCreatedByCurrentUser;
        public bool Private;
        public bool Public;
        public int PersonalCorrectnessProbability;

        // Flags: If all Flags are true, 
        // question will be added to learning session
        public bool AddByKnowledgeSummary;
        public bool AddByWuwi;
        public bool AddByCreator;
        public bool AddByVisibility;
    }

    struct KnowledgeSummaryDetail
    {
        public int QuestionId;
        public int PersonalCorrectnessProbability;
    }

    public LearningSessionCreator(SessionUser sessionUser,
        LearningSessionCache learningSessionCache, 
        PermissionCheck permissionCheck,
        SessionUserCache sessionUserCache)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _sessionUserCache = sessionUserCache;
    }

    // For Tests
    public LearningSessionCreator()
    {
    }

    public record struct Step
    {
        public int id { get; set; }
        public AnswerState state { get; set; }
        public int index { get; set; }
        public bool isLastStep { get; set; }
    }

    public record struct LearningSessionResult()
    {
        public int index { get; set; } = 0;
        public Step[] steps { get; set; } = Array.Empty<Step>();
        public Step? currentStep { get; set; } = null;
        public int activeQuestionCount { get; set; } = 0;
        public bool answerHelp { get; set; } = true;
        public bool isInTestMode { get; set; } = false;
        public string? messageKey { get; set; } = null;
    }

    private LearningSessionResult FillLearningSessionResult(LearningSession learningSession, LearningSessionResult result)
    {
        var currentStep = new Step
        {
            state = learningSession.CurrentStep.AnswerState,
            id = learningSession.CurrentStep.Question.Id,
            index = learningSession.CurrentIndex,
            isLastStep = learningSession.TestIsLastStep()
        };

        result.steps = learningSession.Steps.Select((s, index) => new Step
        {
            id = s.Question.Id,
            state = s.AnswerState,
            index = index
        }).ToArray();

        result.activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count();
        result.currentStep = currentStep;
        result.answerHelp = learningSession.Config.AnswerHelp;
        result.isInTestMode = learningSession.Config.IsInTestMode;

        return result;
    }

    public LearningSessionResult GetLearningSessionResult()
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        var result = new LearningSessionResult();

        if (learningSession.Steps.Any())
        {
            var index = learningSession.Steps.IndexOf(s => s.AnswerState == AnswerState.Unanswered);
            learningSession.LoadSpecificQuestion(index);
            result = FillLearningSessionResult(learningSession, result);
        }

        return result;
    }

    public LearningSessionResult GetLearningSessionResult(LearningSessionConfig config)
    {
        var learningSession = GetLearningSession(config);
        var result = new LearningSessionResult();

        if (learningSession.Steps.Any())
        
            result = FillLearningSessionResult(learningSession, result);

        return result;
    }

    private (IList<QuestionCacheItem> allQuestions, bool questionNotInTopic) CheckQuestionInTopic(int topicId,
        int questionId)
    {
        var topic = EntityCache.GetCategory(topicId);
        var allQuestions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId);
        allQuestions = allQuestions.Where(q => q.Id > 0 && _permissionCheck.CanView(q)).ToList();

        bool questionNotInTopic = allQuestions.All(q => q.Id != questionId);

        return (allQuestions,  questionNotInTopic);
    }

    public LearningSessionResult GetLearningSessionResult(LearningSessionConfig config, int questionId)
    {
        var result = new LearningSessionResult();

        if (!_permissionCheck.CanViewQuestion(questionId))
            result.messageKey = FrontendMessageKeys.Info.Question.IsPrivate;

        var (allQuestions, questionNotInTopic) = CheckQuestionInTopic(config.CategoryId, questionId);
        if (questionNotInTopic)
            result.messageKey = FrontendMessageKeys.Info.Question.NotInTopic;

        var learningSession = GetLearningSession(config, questionId, allQuestions);

        if (!learningSession.Steps.Any()) 
            return result;

        if (learningSession.Steps.Any(s => s.Question.Id == questionId))
            learningSession.LoadSpecificQuestion(learningSession.Steps.IndexOf(s => s.Question.Id == questionId));
        else if (result.messageKey == null)
            result.messageKey = FrontendMessageKeys.Info.Question.NotInFilter;

        result = FillLearningSessionResult(learningSession, result);

        return result;
    }

    public LearningSessionResult GetStep(int index)
    {
        var result = new LearningSessionResult();
        var learningSession = _learningSessionCache.GetLearningSession();
        learningSession.LoadSpecificQuestion(index);

        return FillLearningSessionResult(learningSession, result);
    }

    public void LoadDefaultSessionIntoCache(int topicId, int userId = default)
    {
        var config = new LearningSessionConfig
        {
            CategoryId = topicId,
            CurrentUserId = userId
        };
        _learningSessionCache.AddOrUpdate(BuildLearningSession(config));
    }

    public LearningSession GetLearningSession(LearningSessionConfig config)
    {
        var learningSession = BuildLearningSession(config);
        _learningSessionCache.AddOrUpdate(learningSession);
        return _learningSessionCache.GetLearningSession()!;
    }
    public LearningSession GetLearningSession(LearningSessionConfig config, int questionId, IList<QuestionCacheItem> allQuestions)
    {
        var learningSession = BuildLearningSession(config, allQuestions, questionId);
        _learningSessionCache.AddOrUpdate(learningSession);
        return _learningSessionCache.GetLearningSession()!;
    }

    public QuestionCounter GetQuestionCounterForLearningSession(LearningSessionConfig config)
    {
        return BuildLearningSession(config).QuestionCounter;
    }
    public LearningSession BuildLearningSession(LearningSessionConfig config)
    {
        IList<QuestionCacheItem> allQuestions = EntityCache.GetCategory(config.CategoryId)
            .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId)
            .Where(q => q.Id > 0)
            .Where(_permissionCheck.CanView).ToList();

        return BuildLearningSession(config, allQuestions);
    }

    public LearningSession BuildLearningSession(LearningSessionConfig config, IList<QuestionCacheItem> allQuestions, int questionId = 0)
    {
        _learningSessionCache.TryRemove();
        if (_sessionUser.IsLoggedIn && config.CurrentUserId != _sessionUser.UserId)
            config.CurrentUserId = _sessionUser.UserId;

        var questionCounter = new QuestionCounter();
        var allQuestionValuation = _sessionUserCache.GetQuestionValuations(_sessionUser.UserId);

        IList<QuestionCacheItem> filteredQuestions = new List<QuestionCacheItem>();
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails = new List<KnowledgeSummaryDetail>();

        if (_sessionUser.IsLoggedIn)
        {
            foreach (var question in allQuestions)
            {
                var questionProps = BuildQuestionProperties(config, question, allQuestionValuation);

                if (questionProps.AddByWuwi &&
                    questionProps.AddByCreator &&
                    questionProps.AddByVisibility &&
                    questionProps.AddByKnowledgeSummary)
                {
                    AddQuestionToFilteredList(filteredQuestions, questionProps, question, knowledgeSummaryDetails);
                    questionCounter.Max++;
                }
                questionCounter = CountQuestionsForSessionConfig(questionProps, questionCounter);
            }
        }
        else
        {
            filteredQuestions = allQuestions;
            questionCounter.Max = filteredQuestions.Count;
            questionCounter.NotInWuwi = filteredQuestions.Count;
            questionCounter.NotCreatedByCurrentUser = filteredQuestions.Count;
            questionCounter.NotLearned = filteredQuestions.Count;
            questionCounter.Public = filteredQuestions.Count;
        }

        if (questionId == 0 || filteredQuestions.IndexOf(q => q.Id == questionId) < 0)
        {
            filteredQuestions = filteredQuestions.Shuffle();
            filteredQuestions = GetQuestionsByCount(config, filteredQuestions);
            filteredQuestions = SetQuestionOrder(filteredQuestions, config, knowledgeSummaryDetails);

            var steps = filteredQuestions
                .Distinct()
                .Select(q => new LearningSessionStep(q))
                .ToList();

            return new LearningSession(steps, config)
            {
                QuestionCounter = questionCounter
            };
        }

        return BuildLearningSessionAndInsertQuestion(filteredQuestions, config, questionId, knowledgeSummaryDetails, questionCounter);
    }

    private LearningSession BuildLearningSessionAndInsertQuestion(IList<QuestionCacheItem> filteredQuestions, LearningSessionConfig config, 
        int questionId, IList<KnowledgeSummaryDetail> knowledgeSummaryDetails, QuestionCounter questionCounter)
    {
        filteredQuestions = filteredQuestions.Where(q => q.Id != questionId).ToList();
        filteredQuestions = filteredQuestions.Shuffle();

        filteredQuestions = GetQuestionsByCount(config.MaxQuestionCount - 1, filteredQuestions);

        Random random = new Random();
        int randomIndex = random.Next(filteredQuestions.Count);
        filteredQuestions.Insert(randomIndex, EntityCache.GetQuestion(questionId));

        filteredQuestions = SetQuestionOrder(filteredQuestions, config, knowledgeSummaryDetails);

        var learningSessionSteps = filteredQuestions
            .Distinct()
            .Select(q => new LearningSessionStep(q))
        .ToList();

        return new LearningSession(learningSessionSteps, config)
        {
            QuestionCounter = questionCounter
        };
    }

    public QuestionProperties BuildQuestionProperties(
        LearningSessionConfig config, 
        QuestionCacheItem question,
        IList<QuestionValuationCacheItem> allQuestionValuation)
    {
        var questionDetail = new QuestionProperties();

        questionDetail = FilterByCreator(config, question, questionDetail);
        questionDetail = FilterByVisibility(config, question, questionDetail);
        questionDetail = FilterByKnowledgeSummaryAndWuwi(config, question, questionDetail, allQuestionValuation);

        return questionDetail;
    }

    public void InsertNewQuestionToLearningSession(QuestionCacheItem question, int sessionIndex, LearningSessionConfig config)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        var step = new LearningSessionStep(question);

        if (learningSession != null)
        {
            var allQuestionValuation = 
                _sessionUserCache.GetQuestionValuations(config.CurrentUserId);

            var questionDetail = BuildQuestionProperties(config, question, allQuestionValuation);

            learningSession.QuestionCounter = CountQuestionsForSessionConfig(questionDetail, learningSession.QuestionCounter);

            if (questionDetail.AddByWuwi &&
                questionDetail.AddByCreator &&
                questionDetail.AddByVisibility &&
                questionDetail.AddByKnowledgeSummary)
            {
                if (learningSession.Steps.Count > sessionIndex + 1)
                    learningSession.Steps.Insert(sessionIndex + 1, step);
                else 
                    learningSession.Steps.Add(step);
            }

            learningSession.QuestionCounter.Max += 1;
            _learningSessionCache.AddOrUpdate(learningSession);
        }
    }

    private void AddQuestionToFilteredList(IList<QuestionCacheItem> filteredQuestions,
        QuestionProperties questionProperties, 
        QuestionCacheItem question, 
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {

        if (_sessionUser.IsLoggedIn)
            knowledgeSummaryDetails.Add(new KnowledgeSummaryDetail
            {
                PersonalCorrectnessProbability = questionProperties.PersonalCorrectnessProbability,
                QuestionId = question.Id
            });
            
        filteredQuestions.Add(question);
    }

    private IList<QuestionCacheItem> SetQuestionOrder(IList<QuestionCacheItem> questions, 
        LearningSessionConfig config, 
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {
        if (config.QuestionOrder == QuestionOrder.SortByEasiest)
            return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();

        if (config.QuestionOrder == QuestionOrder.SortByHardest)
            return questions.OrderBy(q => q.CorrectnessProbability).ToList();

        if (_sessionUser.IsLoggedIn && config.QuestionOrder == QuestionOrder.SortByPersonalHardest)
        {
            var orderedKnowledgeSummaryDetails = knowledgeSummaryDetails.OrderBy(k => k.PersonalCorrectnessProbability).ToList();
            return questions.OrderBy(q => orderedKnowledgeSummaryDetails.IndexOf(o => q.Id == o.QuestionId)).ToList();
        }

        return questions;
    }

    public static QuestionCounter CountQuestionsForSessionConfig(QuestionProperties questionProperties, QuestionCounter counter)
    {
        if (questionProperties.NotLearned)
            counter.NotLearned++;

        if (questionProperties.NeedsLearning)
            counter.NeedsLearning++;

        if (questionProperties.NeedsConsolidation)
            counter.NeedsConsolidation++;

        if (questionProperties.Solid)
            counter.Solid++;

        if (questionProperties.InWuwi)
            counter.InWuwi++;

        if (questionProperties.NotInWuwi)
            counter.NotInWuwi++;

        if (questionProperties.CreatedByCurrentUser)
            counter.CreatedByCurrentUser++;

        if (questionProperties.NotCreatedByCurrentUser)
            counter.NotCreatedByCurrentUser++;

        if (questionProperties.Public)
            counter.Public++;

        if (questionProperties.Private)
            counter.Private++;

        return counter;
    }

    private static IList<QuestionCacheItem> GetQuestionsByCount(LearningSessionConfig config, IList<QuestionCacheItem> questions)
    {
        if (config.MaxQuestionCount > questions.Count || config.MaxQuestionCount == -1 || config.MaxQuestionCount == 0)
            return questions;

        return questions.Take(config.MaxQuestionCount).ToList();
    }

    private static IList<QuestionCacheItem> GetQuestionsByCount(int maxQuestionCount, IList<QuestionCacheItem> questions)
    {
        if (maxQuestionCount > questions.Count || maxQuestionCount == -1 || maxQuestionCount == 0)
            return questions;

        return questions.Take(maxQuestionCount).ToList();
    }

    public QuestionProperties FilterByCreator_Test(LearningSessionConfig config, QuestionCacheItem questionCacheItem, QuestionProperties questionProperties) =>
        FilterByCreator(config, questionCacheItem, questionProperties);

    private QuestionProperties FilterByCreator(
        LearningSessionConfig config, 
        QuestionCacheItem questionCacheItem,
        QuestionProperties questionProperties)
    {
        var noCreatorSelected = !config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser;

        if (questionCacheItem.CreatorId == config.CurrentUserId)
        {
            if (config.CreatedByCurrentUser || noCreatorSelected)
                questionProperties.AddByCreator = true;

            questionProperties.CreatedByCurrentUser = true;
        }
        else
        {
            if (config.NotCreatedByCurrentUser || noCreatorSelected)
                questionProperties.AddByCreator = true;

            questionProperties.NotCreatedByCurrentUser = true;
        }

        return questionProperties;
    }

    private static QuestionProperties FilterByVisibility(LearningSessionConfig config, QuestionCacheItem question, QuestionProperties questionProperties)
    {
        var noVisibilitySelected = !config.PrivateQuestions && !config.PublicQuestions;

        if (question.Visibility == QuestionVisibility.All)
        {
            if (config.PublicQuestions || noVisibilitySelected)
                questionProperties.AddByVisibility = true;

            questionProperties.Public = true;
        }
        else
        {
            if (config.PrivateQuestions || noVisibilitySelected)
                questionProperties.AddByVisibility = true;

            questionProperties.Private = true;
        }

        return questionProperties;
    }

    private QuestionProperties FilterByKnowledgeSummaryAndWuwi(
        LearningSessionConfig config, 
        QuestionCacheItem question, 
        QuestionProperties questionProperties, 
        IList<QuestionValuationCacheItem> allQuestionValuation
    )
    {
        if (_sessionUser.IsLoggedIn)
        {
            var questionValuation = allQuestionValuation.FirstOrDefault(qv => qv.Question.Id == question.Id);

            questionProperties = FilterByWuwi(questionValuation, config, questionProperties);

            if (questionValuation == null || questionValuation.CorrectnessProbabilityAnswerCount <= 0)
            {
                if (config.NotLearned)
                    questionProperties.AddByKnowledgeSummary = true;

                questionProperties.NotLearned = true;

                if (questionValuation != null)
                    questionProperties.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
                else questionProperties.PersonalCorrectnessProbability = question.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability <= 50)
            {
                if (config.NeedsLearning)
                    questionProperties.AddByKnowledgeSummary = true;

                questionProperties.NeedsLearning = true;
                questionProperties.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability > 50 && questionValuation.CorrectnessProbability < 80)
            {
                if (config.NeedsConsolidation)
                    questionProperties.AddByKnowledgeSummary = true;

                questionProperties.NeedsConsolidation = true;
                questionProperties.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability >= 80)
            {
                if (config.Solid)
                    questionProperties.AddByKnowledgeSummary = true;

                questionProperties.Solid = true;
                questionProperties.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }

        }
        else
        {
            questionProperties.AddByKnowledgeSummary = true;
            questionProperties.NotLearned = true;
        }

        return questionProperties;
    }

    private static QuestionProperties FilterByWuwi(QuestionValuationCacheItem questionValuation, LearningSessionConfig config, QuestionProperties questionProperties)
    {
        if (questionValuation != null && questionValuation.IsInWishKnowledge)
        {
            if (config.InWuwi || !config.InWuwi && !config.NotInWuwi)
                questionProperties.AddByWuwi = true;

            questionProperties.InWuwi = true;
        }
        else
        {
            if (config.NotInWuwi || !config.InWuwi && !config.NotInWuwi)
                questionProperties.AddByWuwi = true;

            questionProperties.NotInWuwi = true;
        }

        return questionProperties;
    }
}
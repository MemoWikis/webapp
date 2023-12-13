using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
public class LearningSessionCreator : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;

    public struct QuestionDetail
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
        public bool FilterByKnowledgeSummary;
        public bool AddByWuwi;
        public bool AddByCreator;
        public bool AddByVisibility;
        public int PersonalCorrectnessProbability;
    }

    struct KnowledgeSummaryDetail
    {
        public int QuestionId;
        public int PersonalCorrectnessProbability;
    }

    public LearningSessionCreator(SessionUser sessionUser,
        LearningSessionCache learningSessionCache, 
        PermissionCheck permissionCheck,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
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

        learningSession.Steps.First(s => s.AnswerState != AnswerState.Unanswered);


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
            result.messageKey = "questionIsPrivate";

        var (allQuestions, questionNotInTopic) = CheckQuestionInTopic(config.CategoryId, questionId);
        if (questionNotInTopic)
            result.messageKey = "questionNotInTopic";

        var learningSession = GetLearningSession(config, questionId, allQuestions);

        if (!learningSession.Steps.Any()) 
            return result;

        if (learningSession.Steps.Any(s => s.Question.Id == questionId))
            learningSession.LoadSpecificQuestion(learningSession.Steps.IndexOf(s => s.Question.Id == questionId));
        else if (result.messageKey == null)
            result.messageKey = "questionNotInFilter";

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

        var questionCounter = new QuestionCounter();
        var allQuestionValuation = _sessionUserCache.GetQuestionValuations(_sessionUser.UserId);

        IList<QuestionCacheItem> filteredQuestions = new List<QuestionCacheItem>();
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails = new List<KnowledgeSummaryDetail>();

        if (_sessionUser.IsLoggedIn)
        {
            foreach (var q in allQuestions)
            {
                var questionDetail = BuildQuestionDetail(config, q, allQuestionValuation);

                if (questionDetail.AddByWuwi &&
                    questionDetail.AddByCreator &&
                    questionDetail.AddByVisibility &&
                    questionDetail.FilterByKnowledgeSummary)
                {
                    AddQuestionToFilteredList(filteredQuestions, questionDetail, q, knowledgeSummaryDetails);
                    questionCounter.Max++;
                }
                questionCounter = CountQuestionsForSessionConfig(questionDetail, questionCounter);
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

    public QuestionDetail BuildQuestionDetail(LearningSessionConfig config, QuestionCacheItem q,
        IList<QuestionValuationCacheItem> allQuestionValuation)
    {
        var questionDetail = new QuestionDetail();

        questionDetail = FilterByCreator(config, q, questionDetail);
        questionDetail = FilterByVisibility(config, q, questionDetail);
        questionDetail = FilterByKnowledgeSummary(config, q, questionDetail, allQuestionValuation);
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

            var questionDetail = BuildQuestionDetail(config, question, allQuestionValuation);

            learningSession.QuestionCounter = CountQuestionsForSessionConfig(questionDetail, learningSession.QuestionCounter);

            if (questionDetail.AddByWuwi &&
                questionDetail.AddByCreator &&
                questionDetail.AddByVisibility &&
                questionDetail.FilterByKnowledgeSummary)
            {
                if (learningSession.Steps.Count > sessionIndex + 1)
                    learningSession.Steps.Insert(sessionIndex + 1, step);
                else learningSession.Steps.Add(step);
            }

            learningSession.QuestionCounter.Max += 1;
            _learningSessionCache.AddOrUpdate(learningSession);
        }
    }

    private void AddQuestionToFilteredList(IList<QuestionCacheItem> filteredQuestions,
        QuestionDetail questionDetail, 
        QuestionCacheItem question, 
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {

        if (_sessionUser.IsLoggedIn)
            knowledgeSummaryDetails.Add(new KnowledgeSummaryDetail
            {
                PersonalCorrectnessProbability = questionDetail.PersonalCorrectnessProbability,
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

    public static QuestionCounter CountQuestionsForSessionConfig(QuestionDetail questionDetail, QuestionCounter counter)
    {
        if (questionDetail.NotLearned)
            counter.NotLearned++;

        if (questionDetail.NeedsLearning)
            counter.NeedsLearning++;

        if (questionDetail.NeedsConsolidation)
            counter.NeedsConsolidation++;

        if (questionDetail.Solid)
            counter.Solid++;

        if (questionDetail.InWuwi)
            counter.InWuwi++;

        if (questionDetail.NotInWuwi)
            counter.NotInWuwi++;

        if (questionDetail.CreatedByCurrentUser)
            counter.CreatedByCurrentUser++;

        if (questionDetail.NotCreatedByCurrentUser)
            counter.NotCreatedByCurrentUser++;

        if (questionDetail.Public)
            counter.Public++;

        if (questionDetail.Private)
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

    private QuestionDetail FilterByCreator(LearningSessionConfig config, 
        QuestionCacheItem q,
        QuestionDetail questionDetail)
    {
        if (q.Creator.Id == config.CurrentUserId)
        {
            if (config.CreatedByCurrentUser || (!config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser))
                questionDetail.AddByCreator = true;

            questionDetail.CreatedByCurrentUser = true;
        }
        else
        {
            if (config.NotCreatedByCurrentUser || !config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser)
                questionDetail.AddByCreator = true;

            questionDetail.NotCreatedByCurrentUser = true;
        }

        return questionDetail;
    }

    private static QuestionDetail FilterByVisibility(LearningSessionConfig config, QuestionCacheItem q, QuestionDetail questionDetail)
    {
        if (q.Visibility == QuestionVisibility.All)
        {
            if (config.PublicQuestions || !config.PrivateQuestions && !config.PublicQuestions)
                questionDetail.AddByVisibility = true;

            questionDetail.Public = true;
        }
        else
        {
            if (config.PrivateQuestions || !config.PrivateQuestions && !config.PublicQuestions)
                questionDetail.AddByVisibility = true;

            questionDetail.Private = true;
        }

        return questionDetail;
    }

    private QuestionDetail FilterByKnowledgeSummary(LearningSessionConfig config, QuestionCacheItem q, QuestionDetail questionDetail, IList<QuestionValuationCacheItem> allQuestionValuation)
    {
        if (_sessionUser.IsLoggedIn)
        {
            var questionValuation = allQuestionValuation.FirstOrDefault(qv => qv.Question.Id == q.Id);

            questionDetail = FilterByWuwi(questionValuation, config, questionDetail);

            if (questionValuation == null || questionValuation.CorrectnessProbabilityAnswerCount <= 0)
            {
                if (config.NotLearned)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.NotLearned = true;

                if (questionValuation != null)
                    questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
                else questionDetail.PersonalCorrectnessProbability = q.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability <= 50)
            {
                if (config.NeedsLearning)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.NeedsLearning = true;
                questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability > 50 && questionValuation.CorrectnessProbability < 80)
            {
                if (config.NeedsConsolidation)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.NeedsConsolidation = true;
                questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability >= 80)
            {
                if (config.Solid)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.Solid = true;
                questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }

        }
        else
        {
            questionDetail.FilterByKnowledgeSummary = true;
            questionDetail.NotLearned = true;
        }

        return questionDetail;
    }

    private static QuestionDetail FilterByWuwi(QuestionValuationCacheItem questionValuation, LearningSessionConfig config, QuestionDetail questionDetail)
    {
        if (questionValuation != null && questionValuation.IsInWishKnowledge)
        {
            if (config.InWuwi || !config.InWuwi && !config.NotInWuwi)
                questionDetail.AddByWuwi = true;

            questionDetail.InWuwi = true;
        }
        else
        {
            if (config.NotInWuwi || !config.InWuwi && !config.NotInWuwi)
                questionDetail.AddByWuwi = true;

            questionDetail.NotInWuwi = true;
        }

        return questionDetail;
    }
}
using System.Linq;
using NHibernate;
using TrueOrFalse;

public class AnswerQuestion : IRegisterAsInstancePerLifetime
{
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly AnswerLog _answerLog;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly ISession _nhibernateSession;
    private readonly AnswerRepo _answerRepo;
    private readonly ProbabilityUpdate_Question _probabilityUpdateQuestion;
    private readonly UpdateQuestionAnswerCount _updateQuestionAnswerCount;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
    private readonly UserReadingRepo _userReadingRepo;

    public AnswerQuestion(QuestionReadingRepo questionReadingRepo,
        AnswerLog answerLog,
        LearningSessionCache learningSessionCache,
        ISession nhibernateSession,
        AnswerRepo answerRepo,
        ProbabilityUpdate_Question probabilityUpdateQuestion,
        UpdateQuestionAnswerCount updateQuestionAnswerCount,
        QuestionValuationRepo questionValuationRepo,
        ProbabilityCalc_Simple1 probabilityCalcSimple1,
        UserReadingRepo userReadingRepo)
    {
        _questionReadingRepo = questionReadingRepo;
        _answerLog = answerLog;
        _learningSessionCache = learningSessionCache;
        _nhibernateSession = nhibernateSession;
        _answerRepo = answerRepo;
        _probabilityUpdateQuestion = probabilityUpdateQuestion;
        _updateQuestionAnswerCount = updateQuestionAnswerCount;
        _questionValuationRepo = questionValuationRepo;
        _probabilityCalcSimple1 = probabilityCalcSimple1;
        _userReadingRepo = userReadingRepo;
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView)
    {

        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerLog.Run(question, 
                answerQuestionResult,
                userId, 
                questionViewGuid, 
                interactionNumber,
                millisecondsSinceQuestionView);
        });
    }

    public AnswerQuestionResult Run(
       int questionId,
       string answer,
       int userId,
       Guid questionViewGuid,
       int interactionNumber,
       int millisecondsSinceQuestionView,
       int learningSessionId,
       Guid stepGuid,
       bool inTestMode = false,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        var learningSession = _learningSessionCache.GetLearningSession();

        var result = Run(questionId, answer, userId, (question, answerQuestionResult) =>
        {
            _answerLog.Run(
                question, 
                answerQuestionResult, 
                userId,
                questionViewGuid,
                interactionNumber,
                millisecondsSinceQuestionView,
                dateCreated: dateCreated);
            if (learningSession != null)
            {
                answerQuestionResult.NewStepAdded = learningSession.AddAnswer(answerQuestionResult);
                answerQuestionResult.NumberSteps = learningSession.Steps.Count;
            }
        });
        return result;
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Guid answerQuestionGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerLog.Run(question, 
                answerQuestionResult, 
                userId, 
                answerQuestionGuid, 
                interactionNumber,
                millisecondsSinceQuestionView,
                dateCreated: dateCreated); 
        });
    }

    public AnswerQuestionResult Run(
        int questionId,
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int? testSessionId,
        int? learningSessionId,
        string learningSessionStepGuid,
        int millisecondsSinceQuestionView = -1,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        if(countLastAnswerAsCorrect && countUnansweredAsCorrect)
            throw new Exception("either countLastAnswerAsCorrect OR countUnansweredAsCorrect should be set to true, not both");

        if (countLastAnswerAsCorrect || countUnansweredAsCorrect)
        {
            var learningSession = _learningSessionCache.GetLearningSession();
            learningSession.SetCurrentStepAsCorrect();

            var answer =   _answerRepo.GetByQuestionViewGuid(questionViewGuid).OrderByDescending(a => a.Id).First();
            answer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            return Run(questionId, "", userId, (question, answerQuestionResult) =>
                    _answerLog.CountLastAnswerAsCorrect(questionViewGuid), countLastAnswerAsCorrect: true);

            // Sl.AnswerRepo.Update(learningSessionStep.AnswerWithInput);
        }
        throw new Exception("neither countLastAnswerAsCorrect or countUnansweredAsCorrect true");
    }

    public AnswerQuestionResult Run(
        int questionId,
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false)
    {
        if (countLastAnswerAsCorrect && countUnansweredAsCorrect)
            throw new Exception("either countLastAnswerAsCorrect OR countUnansweredAsCorrect should be set to true, not both");

        if (countLastAnswerAsCorrect || countUnansweredAsCorrect)
        {
            var learningSession = _learningSessionCache.GetLearningSession();
            learningSession.SetCurrentStepAsCorrect();

            var answer = _answerRepo.GetByQuestionViewGuid(questionViewGuid).OrderByDescending(a => a.Id).First();
            answer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            return Run(questionId, "", userId, (question, answerQuestionResult) =>
                _answerLog.CountLastAnswerAsCorrect(questionViewGuid), countLastAnswerAsCorrect: true);

            // Sl.AnswerRepo.Update(learningSessionStep.AnswerWithInput);
        }
        throw new Exception("neither countLastAnswerAsCorrect or countUnansweredAsCorrect true");
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Action<Question, AnswerQuestionResult> action,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false)
    {
        var question = _questionReadingRepo.GetById(questionId);
        var questionCacheItem = EntityCache.GetQuestion(questionId);
        var solution = GetQuestionSolution.Run(questionCacheItem);

        var result = new AnswerQuestionResult
        {
            IsCorrect = solution.IsCorrect(answer),
            CorrectAnswer = solution.CorrectAnswer(),
            AnswerGiven = answer
        };

        action(question, result);

        _probabilityUpdateQuestion.Run(question);
        if (countLastAnswerAsCorrect)
            _updateQuestionAnswerCount.ChangeOneWrongAnswerToCorrect(questionId);
        else
            _updateQuestionAnswerCount.Run(questionId, countUnansweredAsCorrect || result.IsCorrect);

        ProbabilityUpdate_Valuation.Run(questionId, userId, _nhibernateSession, _questionReadingRepo, _userReadingRepo, _questionValuationRepo, _probabilityCalcSimple1, _answerRepo);

        return result;
    }
}
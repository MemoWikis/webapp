using TrueOrFalse;
using ISession = NHibernate.ISession;

public class AnswerQuestion(
    QuestionReadingRepo _questionReadingRepo,
    AnswerLog _answerLog,
    LearningSessionCache _learningSessionCache,
    ISession _nhibernateSession,
    AnswerRepo _answerRepo,
    ProbabilityUpdate_Question _probabilityUpdateQuestion,
    UpdateQuestionAnswerCount _updateQuestionAnswerCount,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    ProbabilityCalc_Simple1 _probabilityCalcSimple1,
    UserReadingRepo _userReadingRepo,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public AnswerQuestionResult Run(
       int questionId,
       string answer,
       int userId,
       Guid questionViewGuid,
       int interactionNumber,
       int millisecondsSinceQuestionView,
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

    public AnswerQuestionResult SetCurrentStepAsCorrect(
        int questionId,
        int userId,
        Guid questionViewGuid,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false)
    {
        if (countLastAnswerAsCorrect && countUnansweredAsCorrect)
            throw new Exception("either countLastAnswerAsCorrect OR countUnansweredAsCorrect should be set to true, not both");

        if (countLastAnswerAsCorrect || countUnansweredAsCorrect)
        {
            var learningSession = _learningSessionCache.GetLearningSession();
            if (learningSession == null)
                throw new Exception("learningSession is null");

            learningSession.SetCurrentStepAsCorrect();

            var answer = _answerRepo.GetByQuestionViewGuid(questionViewGuid).OrderByDescending(a => a.Id).First();
            answer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;

            AnswerCache.UpdateAnswerInCache(_extendedUserCache, answer);
            return Run(questionId, "", userId, (question, answerQuestionResult) =>
                _answerLog.CountLastAnswerAsCorrect(questionViewGuid), countLastAnswerAsCorrect: true);
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

        new ProbabilityUpdate_Valuation(_nhibernateSession,
            _questionValuationReadingRepo,
            _probabilityCalcSimple1,
            _answerRepo)
            .Run(questionId, userId, _questionReadingRepo, _userReadingRepo);

        return result;
    }
}
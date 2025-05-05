using NHibernate.Util;
using ISession = NHibernate.ISession;

public class ProbabilityUpdate_Valuation
{
    private readonly ISession _session;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
    private readonly AnswerRepo _answerRepo;

    public ProbabilityUpdate_Valuation(
        ISession session,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        ProbabilityCalc_Simple1 probabilityCalcSimple1,
        AnswerRepo answerRepo)
    {
        _session = session;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _probabilityCalcSimple1 = probabilityCalcSimple1;
        _answerRepo = answerRepo;
    }

    public void Run(int userId)
    {
        _questionValuationReadingRepo
            .GetByUser(userId, onlyActiveKnowledge: false)
            .ForEach(qv => Run(qv));
    }

    private void Run(QuestionValuation questionValuation)
    {
        UpdateValuationProbabilitys(questionValuation);

        _questionValuationReadingRepo.CreateOrUpdate(questionValuation);
    }

    public void Run(
        int questionId,
        int userId,
        QuestionReadingRepo questionReadingRepo,
        UserReadingRepo userReadingRepo)
    {
        var user = userReadingRepo.GetById(userId);

        if (user == null)
            return;

        Run(EntityCache.GetQuestion(questionId),
            user,
            questionReadingRepo
        );
    }

    public void Run(
        QuestionCacheItem question,
        User user,
        QuestionReadingRepo questionReadingRepo)
    {
        var questionValuation =
            _questionValuationReadingRepo.GetBy(question.Id, user.Id) ??
            new QuestionValuation
            {
                Question = questionReadingRepo.GetById(question.Id),
                User = user
            };

        Run(questionValuation);
    }

    private void UpdateValuationProbabilitys(QuestionValuation questionValuation)
    {
        var question = questionValuation.Question;
        var user = EntityCache.GetUserById(questionValuation.User.Id);

        var probabilityResult = _probabilityCalcSimple1
            .Run(EntityCache.GetQuestionById(question.Id)
                , user, _session, _answerRepo);

        questionValuation.CorrectnessProbability = probabilityResult.Probability;
        questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
        questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
    }
}
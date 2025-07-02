using NHibernate.Util;
using ISession = NHibernate.ISession;

public class ProbabilityUpdate_Valuation(
    ISession _session,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    ProbabilityCalc_Simple1 _probabilityCalcSimple1,
    AnswerRepo _answerRepo,
    ExtendedUserCache _extendedUserCache)
{
    public void Run(int userId)
    {
        _questionValuationReadingRepo
            .GetByUser(userId, onlyActiveKnowledge: false)
            .ForEach(qv => Run(qv));
    }

    private void Run(QuestionValuation questionValuation)
    {
        UpdateValuationProbability(questionValuation);

        _extendedUserCache.AddOrUpdate(questionValuation.ToCacheItem());
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

    private void UpdateValuationProbability(QuestionValuation questionValuation)
    {
        var question = questionValuation.Question;
        var user = EntityCache.GetUserById(questionValuation.User.Id);

        var probabilityResult = _probabilityCalcSimple1
            .Run(EntityCache.GetQuestionById(question.Id), user, _session, _answerRepo);

        questionValuation.CorrectnessProbability = probabilityResult.Probability;
        questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
        questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
    }
}
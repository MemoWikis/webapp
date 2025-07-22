using ISession = NHibernate.ISession;

/// <summary>
/// Updates the probabilities 
///     - for all question valuations 
///     - for all useres
/// </summary>
public class ProbabilityUpdate_ValuationAll(
    ISession _nhibernateSession,
    QuestionReadingRepo _questionReadingRepo,
    ProbabilityCalc_Simple1 _probabilityCalcSimple1,
    AnswerRepo _answerRepo,
    UserReadingRepo _userReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    ExtendedUserCache _extendedUserCache)
    : IRegisterAsInstancePerLifetime
{
    public void Run()
    {
        var questionValuationRecords =
            _nhibernateSession.QueryOver<QuestionValuation>()
                .Select(
                    qv => qv.Question.Id,
                    qv => qv.User.Id)
                .List<object[]>();

        foreach (var item in questionValuationRecords)
            new ProbabilityUpdate_Valuation(_nhibernateSession,
                    _questionValuationReadingRepo,
                    _probabilityCalcSimple1,
                    _answerRepo,
                    _extendedUserCache)
                .Run((int)item[0],
                    (int)item[1],
                    _questionReadingRepo,
                    _userReadingRepo);
    }
}
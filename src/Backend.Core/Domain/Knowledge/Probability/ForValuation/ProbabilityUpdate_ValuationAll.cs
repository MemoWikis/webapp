using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

/// <summary>
/// Updates the probabilities 
///     - for all question valuations 
///     - for all useres
/// </summary>
public class ProbabilityUpdate_ValuationAll : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
    private readonly AnswerRepo _answerRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProbabilityUpdate_ValuationAll(ISession nhibernateSession,
        QuestionReadingRepo questionReadingRepo,
        ProbabilityCalc_Simple1 probabilityCalcSimple1,
        AnswerRepo answerRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _nhibernateSession = nhibernateSession;
        _questionReadingRepo = questionReadingRepo;
        _probabilityCalcSimple1 = probabilityCalcSimple1;
        _answerRepo = answerRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
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
                    _answerRepo)
                .Run((int)item[0],
                    (int)item[1],
                    _questionReadingRepo,
                    _userReadingRepo);
    }
}
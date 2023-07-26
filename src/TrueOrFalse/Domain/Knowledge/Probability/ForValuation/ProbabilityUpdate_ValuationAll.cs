using NHibernate;

namespace TrueOrFalse
{
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
        private readonly QuestionValuationRepo _questionValuationRepo;

        public ProbabilityUpdate_ValuationAll(ISession nhibernateSession,
            QuestionReadingRepo questionReadingRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo,
            UserReadingRepo userReadingRepo,
            QuestionValuationRepo questionValuationRepo)
        {
            _nhibernateSession = nhibernateSession;
            _questionReadingRepo = questionReadingRepo;
            _probabilityCalcSimple1 = probabilityCalcSimple1;
            _answerRepo = answerRepo;
            _userReadingRepo = userReadingRepo;
            _questionValuationRepo = questionValuationRepo;
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
                ProbabilityUpdate_Valuation.Run((int) item[0],
                    (int) item[1],
                    _nhibernateSession,
                    _questionReadingRepo,
                    _userReadingRepo,
                    _questionValuationRepo,
                    _probabilityCalcSimple1,
                    _answerRepo);   
        }
    }
}

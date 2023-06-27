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

        public ProbabilityUpdate_ValuationAll(ISession nhibernateSession)
        {
            _nhibernateSession = nhibernateSession;
        }
        public void Run()
        {
            var questionValuationRecords =
                Sl.R<ISession>().QueryOver<QuestionValuation>()
                    .Select(
                        qv => qv.Question.Id,
                        qv => qv.User.Id)
                    .List<object[]>();

            foreach (var item in questionValuationRecords)
                ProbabilityUpdate_Valuation.Run((int) item[0], (int) item[1], _nhibernateSession);   
        }
    }
}

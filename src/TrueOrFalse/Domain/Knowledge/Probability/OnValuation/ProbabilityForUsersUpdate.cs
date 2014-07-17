using NHibernate;

namespace TrueOrFalse
{
    /// <summary>
    /// Updates the probabilities 
    ///     - for all question valuations 
    ///     - for all useres
    /// </summary>
    public class ProbabilityForUsersUpdate : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly ProbabilityForUserUpdate _probabilityForUserUpdate;

        public ProbabilityForUsersUpdate(
            ISession session, 
            ProbabilityForUserUpdate probabilityForUserUpdate)
        {
            _session = session;
            _probabilityForUserUpdate = probabilityForUserUpdate;
        }

        public void Run()
        {
            var questionValuationRecords =
                _session.QueryOver<QuestionValuation>()
                    .Where(qv => qv.RelevancePersonal >= 0)
                    .Select(
                        qv => qv.QuestionId,
                        qv => qv.UserId)
                    .List<object[]>();

            foreach (var item in questionValuationRecords)
                _probabilityForUserUpdate.Run((int) item[0], (int) item[1]);   
        }
    }
}

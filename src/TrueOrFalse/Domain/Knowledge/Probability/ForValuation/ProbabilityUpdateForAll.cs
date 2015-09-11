using NHibernate;

namespace TrueOrFalse
{
    /// <summary>
    /// Updates the probabilities 
    ///     - for all question valuations 
    ///     - for all useres
    /// </summary>
    public class ProbabilityUpdateForAll : IRegisterAsInstancePerLifetime
    {
        public void Run()
        {
            var questionValuationRecords =
                Sl.R<ISession>().QueryOver<QuestionValuation>()
                    .Select(
                        qv => qv.Question.Id,
                        qv => qv.User.Id)
                    .List<object[]>();

            foreach (var item in questionValuationRecords)
                Sl.R<ProbabilityUpdate>().Run((int) item[0], (int) item[1]);   
        }
    }
}

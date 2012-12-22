using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionSetRepository : RepositoryDb<QuestionSet>
    {
        public QuestionSetRepository(ISession session)
            : base(session)
        {
        }
    }
}

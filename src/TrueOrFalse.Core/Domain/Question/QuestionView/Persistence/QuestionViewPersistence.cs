using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class QuestionViewRepository : RepositoryDb<QuestionView> 
    {
        public QuestionViewRepository(ISession session) : base(session) { }

        public int GetViewCount(int questionId)
        {
            return _session.QueryOver<QuestionView>()
                .Select(Projections.RowCount())
                .Where(x => x.QuestionId == questionId)
                .FutureValue<int>()
                .Value;
        }
    }
}

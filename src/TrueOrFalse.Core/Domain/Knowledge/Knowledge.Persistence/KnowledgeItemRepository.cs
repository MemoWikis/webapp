using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class KnowledgeItemRepository : RepositoryDb<KnowledgeItem>
    {
        public KnowledgeItemRepository(ISession session) : base(session){}

        public KnowledgeItem GetBy(int questionId, int userId)
        {
            return _session.QueryOver<KnowledgeItem>()
                           .Where(ki => ki.QuestionId == questionId && ki.UserId == userId)
                           .SingleOrDefault<KnowledgeItem>();
        }
    }
}

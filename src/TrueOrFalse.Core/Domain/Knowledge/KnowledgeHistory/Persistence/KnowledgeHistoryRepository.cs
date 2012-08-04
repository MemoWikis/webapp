using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class KnowledgeHistoryRepository : RepositoryDb<KnowledgeHistoryItem> 
    {
        public KnowledgeHistoryRepository(ISession session) : base(session){}
    }
}

using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class KnowledgeItemRepository : RepositoryDb<KnowledgeItem>
    {
        public KnowledgeItemRepository(ISession session) : base(session){}
    }
}

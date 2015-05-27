using NHibernate;
using Seedworks.Lib.Persistence;

public class KnowledgeHistoryRepository : RepositoryDb<KnowledgeHistoryItem> 
{
    public KnowledgeHistoryRepository(ISession session) : base(session){}
}
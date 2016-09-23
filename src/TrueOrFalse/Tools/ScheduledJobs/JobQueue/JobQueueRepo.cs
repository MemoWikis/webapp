using System.Collections;
using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

public class JobQueueRepo : RepositoryDb<JobQueue>
{
    public JobQueueRepo(ISession session) : base(session)
    {
    }

    public void Add(JobQueueType jobQueueType, string jobContent)
    {
        Session.Save(new JobQueue
        {
            JobQueueType = jobQueueType,
            JobContent = jobContent
        });
        Session.Flush();
    }

    public IList<int> GetReputationUpdateUsers()
    {
        var result = new List<int>();


    }
}
    
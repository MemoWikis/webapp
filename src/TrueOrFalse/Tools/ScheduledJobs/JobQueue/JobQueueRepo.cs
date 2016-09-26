using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void DeleteById(IList<int> jobIds)
    {
        string tmp = jobIds.Select(j => j.ToString()).Aggregate((a, b) => a + ", " + b);
        Logg.r().Information("query: " + tmp);
        _session.CreateSQLQuery("DELETE FROM jobqueue WHERE jobqueue.Id IN (" + tmp +")") // [todo: error with variable qJobIds; either solve on sql-part (handled as double), or use diff. syntax for inserting
                .ExecuteUpdate(); //old: string.Join(",", jobIds) //older: .SetParameter("qJobIds", tmp)

        //Vorlage: userIds.Select(u => u.ToString()).Aggregate((a, b) => a + "," + b));
    }

    public IList<JobQueue> GetReputationUpdateUsers()
    {
        return 
            _session.QueryOver<JobQueue>().Where(j => j.JobQueueType == JobQueueType.UpdateReputationForUser).List();


    }
}
    
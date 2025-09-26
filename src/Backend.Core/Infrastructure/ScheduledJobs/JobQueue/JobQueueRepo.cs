using NHibernate;
using NHibernate.Transform;

public class JobQueueRepo : RepositoryDb<JobQueue>
{
    public JobQueueRepo(ISession session) : base(session)
    {
    }

    public void Add(JobQueueType jobQueueType, string jobContent, int priority = 0)
    {
        Session.Save(new JobQueue
        {
            JobQueueType = jobQueueType,
            JobContent = jobContent,
            Priority = priority,
            DateCreated = DateTime.Now
        });
        Session.Flush();
    }

    public void DeleteById(IList<int> jobIds)
    {
        var query = $"DELETE FROM jobqueue WHERE jobqueue.Id IN ({string.Join(", ", jobIds)})";
        _session.CreateSQLQuery(query).ExecuteUpdate();
    }

    public void DeleteById(int jobId)
    {
        if (jobId < 0)
            return;

        var query = _session
            .CreateSQLQuery("DELETE FROM jobqueue WHERE Id = :jobId")
            .SetParameter("jobId", jobId);

        query.ExecuteUpdate();
    }

    public void DeleteAllJobs(JobQueueType jobQueueType)
    {
        var query = @"DELETE FROM jobqueue WHERE jobqueue.JobQueueType = :jobqueueType";
        _session
            .CreateSQLQuery(query)
            .SetParameter("jobqueueType", (int)jobQueueType)
            .ExecuteUpdate();
    }

    public IList<JobQueue> GetReputationUpdateUsers()
    {
        return
            _session
                .QueryOver<JobQueue>()
                .Where(j => j.JobQueueType == JobQueueType.UpdateReputationForUser)
                .List();
    }

    public IList<JobQueue> GetRecalcKnowledgeSummariesForPage()
    {
        return
            _session
                .QueryOver<JobQueue>()
                .Where(j => j.JobQueueType == JobQueueType.RecalcKnowledgeSummaryForPage)
                .List();
    }

    public JobQueue? GetTopPriorityMailMessage()
    {
        var result = _session
            .CreateSQLQuery(
                @"SELECT 
                Id, 
                JobQueueType, 
                JobContent
            FROM
                jobqueue
            WHERE
                JobQueueType = 5
                AND Priority = (
                    SELECT MAX(Priority)
                    FROM jobqueue
                    WHERE JobQueueType = 5
                )
            LIMIT 1;"
            );

        var mailJobs = result?
            .SetResultTransformer(Transformers.AliasToBean(typeof(JobQueue)))
            .List<JobQueue>();

        if (mailJobs?.Any() == true)
        {
            return mailJobs[0];
        }

        return null;
    }

}

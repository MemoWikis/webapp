using NHibernate;
using NHibernate.Transform;
using NHibernate.Util;
using Seedworks.Lib.Persistence;

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
            Priority = priority
        });
        Session.Flush();
    }

    public void DeleteById(IList<int> jobIds)
    {
        var query = $"DELETE FROM jobqueue WHERE jobqueue.Id IN ({string.Join(", ", jobIds)})";
        _session.CreateSQLQuery(query).ExecuteUpdate();
    }

    public void DeleteAllJobs(JobQueueType jobQueueType)
    {
        var query = $"DELETE FROM jobqueue WHERE jobqueue.JobQueueType = {(int)jobQueueType}";
        _session
            .CreateSQLQuery(query)
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

    public IList<JobQueue> GetRecalcKnowledgeSummariesForCategory()
    {
        return
            _session
                .QueryOver<JobQueue>()
                .Where(j => j.JobQueueType == JobQueueType.RecalcKnowledgeSummaryForCategory)
                .List();
    }

    public IList<JobQueue> GetRemoveQuestionsInCategoryFromWishKnowledge()
    {
        return
            _session
                .QueryOver<JobQueue>()
                .Where(j => j.JobQueueType == JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge)
                .List();
    }
    public JobQueue GetTopPriorityMailMessage()
    {
        var result = _session
            .CreateSQLQuery(
                @"SELECT 
                    Id, JobQueueType, JobContent
                FROM
                    jobqueue
                WHERE
                    Priority = (SELECT 
                    MAX(Priority)
                FROM
                    jobqueue)
                LIMIT 1;"
                );

        var mailJobs = result?
            .SetResultTransformer(Transformers.AliasToBean(typeof(JobQueue)))
            .List();

        if (mailJobs?.Any() == true)
        {
            return (JobQueue)mailJobs[0];
        }

        return null;
    }
}

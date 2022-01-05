using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Transform;
using RollbarSharp;
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
        _session.CreateSQLQuery(query).ExecuteUpdate();
    }

    public IList<JobQueue> GetReputationUpdateUsers()
    {
        return
            _session.QueryOver<JobQueue>().Where(j => j.JobQueueType == JobQueueType.UpdateReputationForUser).List();
    }

    public IList<JobQueue> GetRecalcKnowledgeSummariesForCategory()
    {
        return
            _session.QueryOver<JobQueue>().Where(j => j.JobQueueType == JobQueueType.RecalcKnowledgeSummaryForCategory).List();
    }

    public IList<JobQueue> GetAddCategoryToWishKnowledge()
    {
        return
            _session.QueryOver<JobQueue>()
                .Where(j => j.JobQueueType == JobQueueType.AddCategoryToWishKnowledge).List();
    }

    public IList<JobQueue> GetRemoveQuestionsInCategoryFromWishKnowledge()
    {
        return
            _session.QueryOver<JobQueue>()
                .Where(j => j.JobQueueType == JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge).List();
    }
    public JobQueue GetTopPriorityMailMessage()
    {
        //Sl.Resolve<ISession>()
        //    .CreateSQLQuery(
        //       @"SELECT * FROM memucho_test.jobqueue ORDER BY Priority DESC;");
        var result = Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"SELECT 
                    Id, JobQueueType, JobContent
                FROM
                    memucho_test.jobqueue
                WHERE
                    Priority = (SELECT 
                    MAX(Priority)
                FROM
                    memucho_test.jobqueue)
                LIMIT 1;"
                ).SetResultTransformer(Transformers.AliasToBean(typeof(JobQueue))).List();
        if (result.Count == 0)
        {
            return null;
        }
        return (JobQueue)result[0];
    }
}

using System;
using NHibernate;
using Seedworks.Lib.Persistence;

public class RunningJobRepo : RepositoryDb<RunningJob>
{
    public RunningJobRepo(ISession session) : base(session)
    {
    }

    public bool IsJobRunning(string jobName)
    {
        try
        {
            var jobCount = Session.QueryOver<RunningJob>()
                .Where(x => x.Name == jobName)
                .RowCount();

            if (jobCount == 0)
                return false;

            if (jobCount == 1)
                return true;

            Logg.r().Error("Unexpected job count {JobCount} {Jobname}", jobCount, jobName);

            return true;
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Error in IsJobRunning.");
            return true;
        }
    }

    public void Add(string jobName)
    {
        Session.Save(new RunningJob {
            Name = jobName,
            StartAt = DateTimeX.Now()});
        Session.Flush();
    }

    public void Remove(string jobName)
    {
        var jobs = Session
            .QueryOver<RunningJob>()
            .Where(x => x.Name == jobName)
            .List();

        if(jobs.Count == 0)
            Logg.r().Error("No job for removal found {Jobname}", jobName);

        else if(jobs.Count > 1)
            Logg.r().Error("More than one job for remove found: {Jobname} {JobCount}", jobName, jobs.Count);

        foreach (var job in jobs)
            Delete(job);

        Session.Flush();
    }

    public void TruncateTable()
    {
        _session.CreateSQLQuery("truncate table runningjob").ExecuteUpdate();
    }
}
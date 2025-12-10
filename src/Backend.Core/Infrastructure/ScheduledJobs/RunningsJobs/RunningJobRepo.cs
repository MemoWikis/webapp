using ISession = NHibernate.ISession;

public class RunningJobRepo(ISession session) : RepositoryDb<RunningJob>(session)
{
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

            Log.Error("Unexpected job count {JobCount} {Jobname}", jobCount, jobName);

            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error in IsJobRunning.");
            return true;
        }
    }

    public void Add(string jobName)
    {
        Session.Save(new RunningJob
        {
            Name = jobName,
            StartAt = DateTimeX.Now()
        });
        Session.Flush();
    }

    public void Remove(string jobName)
    {
        var jobs = Session
            .QueryOver<RunningJob>()
            .Where(x => x.Name == jobName)
            .List();

        if (jobs.Count == 0)
            Log.Error("No job for removal found {Jobname}", jobName);

        else if (jobs.Count > 1)
            Log.Error("More than one job for remove found: {Jobname} {JobCount}", jobName, jobs.Count);

        foreach (var job in jobs)
            Delete(job);

        Session.Flush();
    }

    public void TruncateTable()
    {
        _session.CreateSQLQuery("truncate table runningjob").ExecuteUpdate();
    }

    public IList<RunningJob> GetAllRunningJobs()
    {
        return Session.QueryOver<RunningJob>()
            .OrderBy(x => x.StartAt).Desc
            .List();
    }

    public void RemoveJobById(int jobId)
    {
        var job = Session.Get<RunningJob>(jobId);
        if (job != null)
        {
            Delete(job);
            Session.Flush();
            Log.Information("Manually removed running job {JobName} (ID: {JobId})", job.Name, jobId);
        }
        else
        {
            Log.Warning("Attempted to remove non-existent job with ID {JobId}", jobId);
        }
    }

    public void RemoveStuckJobs(double maxHours = 2.0)
    {
        var cutoffTime = DateTimeX.Now().AddHours(-maxHours);
        var stuckJobs = Session.QueryOver<RunningJob>()
            .Where(x => x.StartAt < cutoffTime)
            .List();

        foreach (var job in stuckJobs)
        {
            Delete(job);
            Log.Information("Removed stuck job {JobName} running for {Duration} hours", 
                job.Name, (DateTimeX.Now() - job.StartAt).TotalHours);
        }

        if (stuckJobs.Any())
        {
            Session.Flush();
            Log.Information("Removed {Count} stuck jobs older than {Hours} hours", stuckJobs.Count, maxHours);
        }
    }

    public int RemoveJobsByIds(List<int> jobIds)
    {
        var removedCount = 0;
        
        foreach (var jobId in jobIds)
        {
            var job = Session.Get<RunningJob>(jobId);
            if (job != null)
            {
                Delete(job);
                Log.Information("Removed running job {JobName} (ID: {JobId})", job.Name, jobId);
                removedCount++;
            }
            else
            {
                Log.Warning("Job with ID {JobId} not found for removal", jobId);
            }
        }

        if (removedCount > 0)
        {
            Session.Flush();
            Log.Information("Removed {Count} jobs by ID", removedCount);
        }

        return removedCount;
    }
}
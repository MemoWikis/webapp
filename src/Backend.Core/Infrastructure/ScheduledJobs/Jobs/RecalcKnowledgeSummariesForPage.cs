using Autofac;
using Quartz;

public class RecalcKnowledgeSummariesForPage : IJob
{
    private readonly KnowledgeSummaryUpdate _knowledgeSummaryUpdate;

    public RecalcKnowledgeSummariesForPage(KnowledgeSummaryUpdate knowledgeSummaryUpdate)
    {
        _knowledgeSummaryUpdate = knowledgeSummaryUpdate;
    }

    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            List<int> successfullJobIds = new List<int>();
            var jobs = scope.Resolve<JobQueueRepo>().GetRecalcKnowledgeSummariesForPage();
            var jobsByPageId = jobs.GroupBy(j => j.JobContent);
            foreach (var grouping in jobsByPageId)
            {
                try
                {
                    _knowledgeSummaryUpdate.RunForPage(Convert.ToInt32(grouping.Key));

                    successfullJobIds.AddRange(grouping.Select(j => j.Id).ToList<int>());
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error in job RecalcKnowledgeSummariesForPage.");
                }
            }

            //Delete jobs that have been executed successfully
            if (successfullJobIds.Count > 0)
            {
                scope.Resolve<JobQueueRepo>().DeleteById(successfullJobIds);
                Log.Information("Job RecalcKnowledgeSummariesForPage recalculated knowledge summary for " + successfullJobIds.Count + " jobs.");
                successfullJobIds.Clear();
            }
        }, "RecalcKnowledgeSummariesForPage");
        return Task.CompletedTask;
    }
}
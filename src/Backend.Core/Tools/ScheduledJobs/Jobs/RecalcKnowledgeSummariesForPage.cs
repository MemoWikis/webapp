﻿using Autofac;
using Quartz;

public class RecalcKnowledgeSummariesForPage : IJob
{
    private readonly PageValuationReadingRepository _pageValuationReadingRepository;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly PageValuationWritingRepo _pageValuationWritingRepo;

    public RecalcKnowledgeSummariesForPage(PageValuationReadingRepository pageValuationReadingRepository,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        PageValuationWritingRepo pageValuationWritingRepo)
    {
        _pageValuationReadingRepository = pageValuationReadingRepository;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _pageValuationWritingRepo = pageValuationWritingRepo;
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
                    KnowledgeSummaryUpdate.RunForPage(Convert.ToInt32(grouping.Key),
                        _pageValuationReadingRepository,
                        _pageValuationWritingRepo,
                        _knowledgeSummaryLoader);

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
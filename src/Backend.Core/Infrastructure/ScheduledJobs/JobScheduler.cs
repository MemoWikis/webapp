using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

public static class JobScheduler
{
    private static IScheduler _scheduler = null!;
    public static string MmapCacheDailyRefreshTrackingId = "mmapCacheDailyRefreshTrackingId";

    public static void Clear() => _scheduler.Clear();

    public static async Task InitializeAsync()
    {
        Log.Information("JobScheduler.InitializeAsync");

        var container = AutofacWebInitializer.Run();
        _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        _scheduler.JobFactory = new AutofacJobFactory(container);
        await _scheduler.Start();
    }

    public static async Task Start(RunningJobRepo runningJobRepo)
    {
        await InitializeAsync();

        //runningJobRepo.TruncateTable();

        //Schedule_RecalcKnowledgeStati();
        //Schedule_RecalcKnowledgeSummariesForPage();
        //Schedule_RecalcReputation();
        //Schedule_RecalcReputationForAll();
        //Schedule_KnowledgeReportCheck();
        //Schedule_RecalcTotalWishInOthersPeople();
        //Schedule_MailSender();
        Schedule_MmapCacheRefresh();
    }


    //private static void Schedule_RecalcKnowledgeStati()
    //{
    //    _scheduler.ScheduleJob(JobBuilder.Create<RecalcKnowledgeStati>().Build(),
    //        TriggerBuilder.Create()
    //            .WithDailyTimeIntervalSchedule(x =>
    //                x.StartingDailyAt(new TimeOfDay(2, 00))
    //                    .OnEveryDay()
    //                    .EndingDailyAfterCount(1)).Build());
    //}

    //private static void Schedule_RecalcKnowledgeSummariesForPage()
    //{
    //    _scheduler.ScheduleJob(JobBuilder.Create<RecalcKnowledgeSummariesForPage>().Build(),
    //        TriggerBuilder.Create()
    //            .WithSimpleSchedule(x => x
    //                .WithIntervalInSeconds(RecalcReputation.IntervalInSeconds)
    //                .RepeatForever()).Build());
    //}

    private static void Schedule_MailSender()
    {
        _scheduler.ScheduleJob(JobBuilder.Create<ScheduledMailSender>().Build(),
            TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5)
                    .RepeatForever()).Build());
    }

    private static void Schedule_MmapCacheRefresh()
    {
        var timeOfDay = Settings.Environment == "prod" ? new TimeOfDay(2, 00) : new TimeOfDay(3, 00);
        var jobTrackingId = MmapCacheDailyRefreshTrackingId;

        _scheduler.ScheduleJob(JobBuilder.Create<MmapCacheRefreshJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(x =>
                    x.StartingDailyAt(timeOfDay)
                        .OnEveryDay()
                        .EndingDailyAfterCount(1)).Build());
    }

    //private static void Schedule_RecalcReputation()
    //{
    //    //recalculates reputation for users specified in table jobqueue, which is filled when relevant actions are taken that affect users reputation
    //    _scheduler.ScheduleJob(JobBuilder.Create<RecalcReputation>().Build(),
    //        TriggerBuilder.Create()
    //            .WithSimpleSchedule(x => x
    //                .WithIntervalInSeconds(RecalcReputation.IntervalInSeconds)
    //                .RepeatForever()).Build());
    //}

    //private static void Schedule_RecalcReputationForAll()
    //{
    //    //once a day, recalculate reputation for all users
    //    _scheduler.ScheduleJob(JobBuilder.Create<RecalcReputationForAll>().Build(),
    //        TriggerBuilder.Create()
    //            .WithDailyTimeIntervalSchedule(x =>
    //                x.StartingDailyAt(new TimeOfDay(3, 00))
    //                    .OnEveryDay()
    //                    .EndingDailyAfterCount(1)).Build());
    //}

    //private static void Schedule_RecalcTotalWishInOthersPeople()
    //{
    //    //once a day, recalculate reputation for all users
    //    _scheduler.ScheduleJob(JobBuilder.Create<RecalcTotalWishInOthersPeople>().Build(),
    //        TriggerBuilder.Create()
    //            .WithDailyTimeIntervalSchedule(x =>
    //                x.StartingDailyAt(new TimeOfDay(4, 00))
    //                    .OnEveryDay()
    //                    .EndingDailyAfterCount(1)).Build());
    //}

    //private static void Schedule_KnowledgeReportCheck()
    //{
    //    _scheduler.ScheduleJob(JobBuilder.Create<KnowledgeReportCheck>().Build(),
    //        TriggerBuilder.Create()
    //            .WithDailyTimeIntervalSchedule(x =>
    //                x.StartingDailyAt(new TimeOfDay(10, 00))
    //                    .OnEveryDay()
    //                    .EndingDailyAfterCount(1)).Build());
    //}

    //private static void Schedule_RefreshEntityCache()
    //{
    //    _scheduler.ScheduleJob(JobBuilder.Create<RefreshEntityCache>().Build(),
    //        TriggerBuilder.Create()
    //            .WithDailyTimeIntervalSchedule(x =>
    //                x.StartingDailyAt(new TimeOfDay(3, 30))
    //                    .OnEveryDay()
    //                    .EndingDailyAfterCount(1)).Build());
    //}z

    public static void StartImmediately<TypeToStart>() where TypeToStart : IJob
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<TypeToStart>().Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_DeleteQuestion(int questionId, int userId, string? parentIdString)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<DeleteQuestion>()
                .UsingJobData("questionId", questionId)
                .UsingJobData("userId", userId)
                .UsingJobData("parentIdString", parentIdString)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_SendEmail(string mailJsonString)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<ImmediatelySendEmail>()
                .UsingJobData("mailJsonString", mailJsonString)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_ReindexAllQuestions(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<MeiliReIndexQuestionsJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_RecalculateKnowledgeItems(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<RecalculateKnowledgeItemsJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_CalcAggregatedValues(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<CalcAggregatedValuesJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_UpdateUserReputation(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<UpdateUserReputationJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_MeiliReIndexPages(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<MeiliReIndexPagesJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_MeiliReIndexUsers(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<MeiliReIndexUsersJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_RelationErrorAnalysis(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<RelationErrorAnalysis>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void StartImmediately_MmapCacheRefresh(string jobTrackingId)
    {
        _scheduler.ScheduleJob(
            JobBuilder.Create<MmapCacheRefreshJob>()
                .UsingJobData("jobTrackingId", jobTrackingId)
                .Build(),
            TriggerBuilder.Create().StartNow().Build());
    }

    public static void ScheduleDelayedImageCleanup(int pageId, string[] imageUrls, TimeSpan delay)
    {
        var jobKey = new JobKey($"DelayedImageCleanup_Page_{pageId}_{DateTime.UtcNow.Ticks}", "ImageCleanup");
        var job = JobBuilder.Create<DelayedImageCleanupJob>()
            .WithIdentity(jobKey)
            .UsingJobData("pageId", pageId)
            .UsingJobData("imageUrlsToCheck", string.Join(",", imageUrls))
            .UsingJobData("scheduledAt", DateTime.UtcNow.ToString("O"))
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"DelayedImageCleanupTrigger_Page_{pageId}_{DateTime.UtcNow.Ticks}", "ImageCleanup")
            .StartAt(DateTimeOffset.Now.Add(delay))
            .Build();

        _scheduler.ScheduleJob(job, trigger);
    }

    // Quartz Job Management Methods
    public static async Task<IReadOnlyCollection<IJobExecutionContext>> GetCurrentlyExecutingJobs()
    {
        return await _scheduler.GetCurrentlyExecutingJobs();
    }

    public static async Task<bool> InterruptJob(JobKey jobKey)
    {
        try
        {
            var result = await _scheduler.Interrupt(jobKey);
            Log.Information("Interrupted job {JobKey}: {Success}", jobKey, result);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error interrupting job {JobKey}", jobKey);
            return false;
        }
    }

    public static async Task<bool> InterruptJob(string jobName, string? groupName = null)
    {
        var jobKey = new JobKey(jobName, groupName ?? "DEFAULT");
        return await InterruptJob(jobKey);
    }

    public static async Task<bool> DeleteJob(JobKey jobKey)
    {
        try
        {
            var result = await _scheduler.DeleteJob(jobKey);
            Log.Information("Deleted job {JobKey}: {Success}", jobKey, result);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting job {JobKey}", jobKey);
            return false;
        }
    }
    public static async Task<IReadOnlyCollection<JobKey>> GetAllJobKeys()
    {
        try
        {
            var groupNames = await _scheduler.GetJobGroupNames();
            var allJobKeys = new List<JobKey>();
            
            foreach (var groupName in groupNames)
            {
                var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
                allJobKeys.AddRange(jobKeys);
            }
            
            return allJobKeys.AsReadOnly();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting job keys");
            return new List<JobKey>().AsReadOnly();
        }
    }

    public static async Task<List<QuartzJobInfo>> GetQuartzJobsInfo()
    {
        try
        {
            var executingJobs = await GetCurrentlyExecutingJobs();
            var allJobKeys = await GetAllJobKeys();
            var jobInfos = new List<QuartzJobInfo>();

            // Add currently executing jobs
            foreach (var executingJob in executingJobs)
            {
                jobInfos.Add(new QuartzJobInfo
                {
                    JobKey = executingJob.JobDetail.Key.ToString(),
                    JobName = executingJob.JobDetail.Key.Name,
                    JobGroup = executingJob.JobDetail.Key.Group,
                    JobType = executingJob.JobDetail.JobType.Name,
                    IsExecuting = true,
                    FireTime = executingJob.FireTimeUtc,
                    RunTime = executingJob.JobRunTime
                });
            }

            // Add scheduled but not executing jobs
            foreach (var jobKey in allJobKeys)
            {
                if (!jobInfos.Any(j => j.JobKey == jobKey.ToString()))
                {
                    var jobDetail = await _scheduler.GetJobDetail(jobKey);
                    if (jobDetail != null)
                    {
                        jobInfos.Add(new QuartzJobInfo
                        {
                            JobKey = jobKey.ToString(),
                            JobName = jobKey.Name,
                            JobGroup = jobKey.Group,
                            JobType = jobDetail.JobType.Name,
                            IsExecuting = false,
                            FireTime = null,
                            RunTime = TimeSpan.Zero
                        });
                    }
                }
            }

            return jobInfos;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting Quartz job info");
            return new List<QuartzJobInfo>();
        }
    }

    // Job-specific interrupt methods
    public static void InterruptRecalculateKnowledgeItems()
    {
        RecalculateKnowledgeItemsJob.RequestInterrupt();
        Log.Information("Interrupt requested for RecalculateKnowledgeItemsJob");
    }

    public static void InterruptCalcAggregatedValues()
    {
        CalcAggregatedValuesJob.RequestInterrupt();
        Log.Information("Interrupt requested for CalcAggregatedValuesJob");
    }

    public static void InterruptUpdateUserReputation()
    {
        UpdateUserReputationJob.RequestInterrupt();
        Log.Information("Interrupt requested for UpdateUserReputationJob");
    }

    public static void InterruptRelationErrorAnalysis()
    {
        RelationErrorAnalysis.RequestInterrupt();
        Log.Information("Interrupt requested for RelationErrorAnalysis");
    }

    public static void ResetAllJobInterrupts()
    {
        RecalculateKnowledgeItemsJob.ResetInterrupt();
        CalcAggregatedValuesJob.ResetInterrupt();
        UpdateUserReputationJob.ResetInterrupt();
        RelationErrorAnalysis.ResetInterrupt();
        Log.Information("Reset interrupt flags for all interruptable jobs");
    }
}

public class QuartzJobInfo
{
    public string JobKey { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public string JobGroup { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public bool IsExecuting { get; set; }
    public DateTimeOffset? FireTime { get; set; }
    public TimeSpan RunTime { get; set; }
}
using Quartz;
using Quartz.Impl;

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
}
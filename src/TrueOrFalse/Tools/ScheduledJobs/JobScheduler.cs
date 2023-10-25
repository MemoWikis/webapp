using System.Collections.Generic;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;
using Quartz.Impl;
using TrueOrFalse.Environment;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Tools.ScheduledJobs.Jobs;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public static class JobScheduler
    {
        static readonly IScheduler _scheduler;

        static JobScheduler()
        {
            var context = AutofacWebInitializer.GetContainer().Resolve<IHttpContextAccessor>();
            var webhostEnvironment = WebHostEnvironmentProvider.GetWebHostEnvironment();
            _scheduler = new Lazy<Task<IScheduler>>(InitializeAsync(context, webhostEnvironment)).Value.Result;
        }

        public static void EmptyMethodToCallConstructor()
        {
        }

        public static async Task<IScheduler> InitializeAsync(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            var container = AutofacWebInitializer.Run();
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = new AutofacJobFactory(container, httpContextAccessor, webHostEnvironment);
            scheduler.Start();

            return scheduler;
        }

        public static void Shutdown()
        {
            _scheduler.Shutdown(waitForJobsToComplete:true);
        }

        public static void Start(RunningJobRepo runningJobRepo)
        {
           runningJobRepo.TruncateTable();
            
            Schedule_CleanupWorkInProgressQuestions();
            Schedule_RecalcKnowledgeStati();
            Schedule_RecalcKnowledgeSummariesForCategory();
            Schedule_RecalcReputation();
            Schedule_RecalcReputationForAll();
            Schedule_EditCategoryInWishKnowledge();
            Schedule_KnowledgeReportCheck();
            Schedule_LOM_Export();
            Schedule_RecalcTotalWishInOthersPeople();
            Schedule_MailTransmitter();

        }

        private static void Schedule_CleanupWorkInProgressQuestions()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<CleanUpWorkInProgressQuestions>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(6)
                    .RepeatForever()).Build());
        }

        private static void Schedule_RecalcKnowledgeStati()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcKnowledgeStati>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x => 
                        x.StartingDailyAt(new TimeOfDay(2, 00))
                         .OnEveryDay()
                         .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_RecalcKnowledgeSummariesForCategory()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcKnowledgeSummariesForCategory>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(RecalcReputation.IntervalInSeconds)
                        .RepeatForever()).Build());
        }
        private static void Schedule_MailTransmitter()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<ScheduledMailSender>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(1)
                        .RepeatForever()).Build());
        }

        private static void Schedule_RecalcReputation()
        {
            //recalculates reputation for users specified in table jobqueue, which is filled when relevant actions are taken that affect users reputation
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcReputation>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(RecalcReputation.IntervalInSeconds)
                    .RepeatForever()).Build());
        }

        private static void Schedule_RecalcReputationForAll()
        {
            //once a day, recalculate reputation for all users
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcReputationForAll>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(3, 00))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_RecalcTotalWishInOthersPeople()
        {
            //once a day, recalculate reputation for all users
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcTotalWishInOthersPeople>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(4, 00))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_KnowledgeReportCheck()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<KnowledgeReportCheck>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(10, 00))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_LOM_Export()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<LomExportJob>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(3, 30))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_RefreshEntityCache()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<RefreshEntityCache>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(3, 30))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_EditCategoryInWishKnowledge()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<EditCategoryInWishKnowledge>().Build(),
                TriggerBuilder.Create().
                    WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(EditCategoryInWishKnowledge.IntervalInSeconds)
                            .RepeatForever()).Build());
        }

        public static void StartImmediately_CleanUpWorkInProgressQuestions() { StartImmediately<CleanUpWorkInProgressQuestions>(); }
        public static void StartImmediately_RecalcKnowledgeStati() { StartImmediately<RecalcKnowledgeStati>(); }
        public static void StartImmediately_RefreshEntityCache() { StartImmediately<RefreshEntityCache>(); }
        public static void StartImmediately_RecalcTotalWishInOthersPeople() { StartImmediately<RecalcTotalWishInOthersPeople>(); }

        public static void StartImmediately<TypeToStart>() where TypeToStart : IJob
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<TypeToStart>().Build(),
                TriggerBuilder.Create().StartNow().Build());
        }

        public static void StartImmediately_InitUserValuationCache(int userId)
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<InitUserValuationCache>()
                .UsingJobData("userId", userId)
                .Build(),
                TriggerBuilder.Create().StartNow().Build());
        }

        public static void StartImmediately_UpdateAggregatedCategoriesForQuestion(List<int> categoryIds)
        {
            var job = JobBuilder.Create<UpdateAggregatedCategoriesForQuestion>()
                .Build();

            job.JobDataMap["categoryIds"] = categoryIds;

            _scheduler.ScheduleJob(
                job,
                TriggerBuilder.Create().StartNow().Build());
        }

        public static void StartImmediately_DeleteQuestion(int questionId)
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<DeleteQuestion>()
                    .UsingJobData("questionId", questionId)
                    .Build(),
                TriggerBuilder.Create().StartNow().Build());
        }

        public static void StartImmediately_ModifyCategoryRelation(int childCategoryId, int parentCategoryId)
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<AddParentCategoryInDb>()
                    .UsingJobData("childCategoryId", childCategoryId)
                    .UsingJobData("parentCategoryId", parentCategoryId)
                    .Build(),
                TriggerBuilder.Create().StartNow().Build());
        }
    }
}


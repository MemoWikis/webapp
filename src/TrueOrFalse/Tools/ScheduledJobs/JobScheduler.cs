using System;
using Quartz;
using Quartz.Impl;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public static class JobScheduler
    {
        static readonly IScheduler _scheduler;

        static JobScheduler()
        {
            var container = AutofacWebInitializer.Run();

            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.JobFactory = new AutofacJobFactory(container);
            _scheduler.Start();
        }

        public static void Start()
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<CleanUpWorkInProgressQuestions>().Build(), 
                TriggerBuilder
                    .Create()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(6).RepeatForever())
                    .Build()
            );
             
            _scheduler.ScheduleJob(
                JobBuilder.Create<GameLoop>().Build(),
                TriggerBuilder
                    .Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                    .Build()
            );

            _scheduler.ScheduleJob(
                JobBuilder.Create<RecalcKnowledgeStati>().Build(),
                TriggerBuilder
                    .Create()
                    .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(new TimeOfDay(2, 00)))
                    .Build()
            );
        }

        public static void StartCleanupWorkInProgressJob()
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<CleanUpWorkInProgressQuestions>().Build(),
                TriggerBuilder.Create().StartNow().Build());
        }
    }
}


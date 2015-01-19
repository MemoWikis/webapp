using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public static class JobScheduler
    {
        readonly static IScheduler _scheduler;

        static JobScheduler()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.Start();
        }

        public static void Start()
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<CleanUpWorkInProgressQuestions>().Build(), 
                TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInHours(6)).Build());
        }

        public static void StartCleanupWorkInProgressJob()
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<CleanUpWorkInProgressQuestions>().Build(),
                TriggerBuilder.Create().StartNow().Build());
        }
    }
}


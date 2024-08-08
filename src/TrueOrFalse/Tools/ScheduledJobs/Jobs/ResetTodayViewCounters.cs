using Autofac;
using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class ResetTodayViewCounters : IJob
    {

      
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
               var allCategories = EntityCache.GetAllCategoriesList();
                foreach (var category in allCategories)
                {
                    category.TodayViewCount = 0;
                }
                var allQuestions = EntityCache.GetAllQuestions();
                foreach (var question in allQuestions)
                {
                    question.TodayViewCount = 0;
                }
            }, "ResetTodayViewCount");

            return Task.CompletedTask;
        }
    }
}
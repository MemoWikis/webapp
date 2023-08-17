
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoriesForQuestion : IJob
    {
        private readonly JobQueueRepo _jobQueueRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SessionUser _sessionUser;
        private readonly CategoryRepository _categoryRepository;

        public UpdateAggregatedCategoriesForQuestion(JobQueueRepo jobQueueRepo,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _jobQueueRepo = jobQueueRepo;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public UpdateAggregatedCategoriesForQuestion(SessionUser sessionUser, CategoryRepository categoryRepository)
        {
            _sessionUser = sessionUser;
            _categoryRepository = categoryRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job started - Update Aggregated Categories from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var categoryIds = (List<int>)dataMap["categoryIds"];

            var aggregatedCategoriesToUpdate =
                CategoryAggregation.GetAggregatingAncestors(_categoryRepository.GetByIds(categoryIds), _categoryRepository);

            foreach (var category in aggregatedCategoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated(_sessionUser.UserId);
                _categoryRepository.Update(category);
                KnowledgeSummaryUpdate.ScheduleForCategory(category.Id, _jobQueueRepo);
                new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Update Category from Update Question - {id}", category.Id);
            }

            return Task.CompletedTask;
        }
    }
}

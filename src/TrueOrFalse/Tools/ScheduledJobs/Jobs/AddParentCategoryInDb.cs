using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;
using System.Threading.Tasks;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddParentCategoryInDb : IJob
    {
        private readonly SessionUser _sessionUser;
        private readonly CategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddParentCategoryInDb(SessionUser sessionUser,
            CategoryRepository categoryRepository,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _sessionUser = sessionUser;
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var childCategoryId = dataMap.GetInt("childCategoryId");
            var parentCategoryId = dataMap.GetInt("parentCategoryId");
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
            await Run(childCategoryId, parentCategoryId);
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
        }

        private Task Run(int childCategoryId, int parentCategoryId)
        {
            var childCategory = _categoryRepository.GetById(childCategoryId);
            var parentCategory = _categoryRepository.GetById(parentCategoryId);

            new ModifyRelationsForCategory(_categoryRepository).AddParentCategory(childCategory, parentCategoryId);

             _categoryRepository.Update(childCategory, _sessionUser.User, type: CategoryChangeType.Relations);
             _categoryRepository.Update(parentCategory, _sessionUser.User, type: CategoryChangeType.Relations);
             return Task.CompletedTask;
        }
    }
}
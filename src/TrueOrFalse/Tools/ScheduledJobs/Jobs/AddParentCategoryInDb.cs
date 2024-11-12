using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddParentCategoryInDb : IJob
    {
        private readonly PageRepository _pageRepository;
        private readonly PageRelationRepo _pageRelationRepo;
        private int _authorId; 

        public AddParentCategoryInDb(PageRepository pageRepository, PageRelationRepo pageRelationRepo)
        {
            _pageRepository = pageRepository;
            _pageRelationRepo = pageRelationRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //context.JobDetail.JobDataMap["context"];
            var dataMap = context.JobDetail.JobDataMap;
            var childCategoryId = dataMap.GetInt("childCategoryId");
            var parentCategoryId = dataMap.GetInt("parentCategoryId");
            _authorId = dataMap.GetInt("authorId"); 

            Logg.r.Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
            await Run(childCategoryId, parentCategoryId);
            Logg.r.Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
        }

        private Task Run(int childCategoryId, int parentCategoryId)
        {
            var childCategory = _pageRepository.GetById(childCategoryId);
            var parentCategory = _pageRepository.GetById(parentCategoryId);

            new ModifyRelationsForPage(_pageRepository, _pageRelationRepo).AddParentPage(childCategory, parentCategoryId);

             _pageRepository.Update(childCategory, _authorId, type: PageChangeType.Relations);
             _pageRepository.Update(parentCategory, _authorId, type: PageChangeType.Relations);
             return Task.CompletedTask;
        }
    }
}
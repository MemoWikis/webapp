using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Data;
using NHibernate;
using NHibernate.Mapping;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class ModifyCategoryRelation : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            var dataMap = context.JobDetail.JobDataMap;
            var childCategoryId = dataMap.GetInt("childCategoryId");
            var parentCategoryId = dataMap.GetInt("parentCategoryId");
            Logg.r().Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);

            var catRepo = Sl.CategoryRepo;

            var childCategory = catRepo.GetById(childCategoryId);
            var parentCategory = catRepo.GetById(parentCategoryId);

            ModifyRelationsForCategory.AddParentCategory(childCategory, parentCategoryId);
            ModifyRelationsForCategory.AddCategoryRelationOfType(parentCategory, childCategoryId, CategoryRelationType.IncludesContentOf);

            catRepo.Update(childCategory, Sl.SessionUser.User, type: CategoryChangeType.Relations);
            catRepo.Update(parentCategory, Sl.SessionUser.User, type: CategoryChangeType.Relations);
            Logg.r().Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
        }
    }
}
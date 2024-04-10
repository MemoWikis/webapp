using Org.BouncyCastle.Asn1.Ocsp;
using Quartz;
using System.Text.Json;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddOrUpdateRelationsInDb : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryRelationRepo _categoryRelationRepo;
        private int _authorId; 

        public AddOrUpdateRelationsInDb(CategoryRepository categoryRepository, CategoryRelationRepo categoryRelationRepo)
        {
            _categoryRepository = categoryRepository;
            _categoryRelationRepo = categoryRelationRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var relationsJson = dataMap.GetString("relations");
            var relations = JsonSerializer.Deserialize<List<CategoryCacheRelation>>(relationsJson);
            _authorId = dataMap.GetInt("authorId"); 

            await Run(relations);
            Logg.r.Information("Job ended - ModifyRelations");
        }

        private Task Run(List<CategoryCacheRelation> relations)
        {
            foreach (var r in relations)
            {
                Logg.r.Information("Job started - ModifyRelations RelationId: {relationId}, Child: {childId}, Parent: {parentId}", r.Id, r.ChildId, r.ParentId);

                var relationToUpdate = r.Id > 0 ? _categoryRelationRepo.GetById(r.Id) : null;
                var child = _categoryRepository.GetById(r.ChildId);
                var parent = _categoryRepository.GetById(r.ParentId);

                if (relationToUpdate != null)
                {
                    relationToUpdate.Child = child;
                    relationToUpdate.Parent = parent;
                    relationToUpdate.PreviousId = r.PreviousId;
                    relationToUpdate.NextId = r.NextId;

                    _categoryRelationRepo.Update(relationToUpdate);
                }
                else
                {
                    var relation = new CategoryRelation
                    {
                        Child = child,
                        Parent = parent,
                        PreviousId = r.PreviousId,
                        NextId = r.NextId,
                    };

                    _categoryRelationRepo.Create(relation);
                }
                _categoryRepository.Update(child, _authorId, type: CategoryChangeType.Relations);
                _categoryRepository.Update(parent, _authorId, type: CategoryChangeType.Relations);
            }

            return Task.CompletedTask;
        }
    }
}
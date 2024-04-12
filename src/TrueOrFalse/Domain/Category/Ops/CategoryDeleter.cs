using ISession = NHibernate.ISession;

public class CategoryDeleter(
    ISession _session,
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    CategoryRepository _categoryRepository,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryValuationWritingRepo _categoryValuationWritingRepo,
    PermissionCheck _permissionCheck,
    SessionUserCache _sessionUserCache,
    CrumbtrailService crumbtrailService,
    CategoryRepository categoryRepo,
    CategoryRelationRepo _categoryRelationRepo)
    : IRegisterAsInstancePerLifetime
{
    ///todo:(DaMa)  Revise: Wrong place for SQL commands.
    private async Task<HasDeleted> Run(Category category, int userId, bool isTestCase = false)
    {
        var categoryCacheItem = EntityCache.GetCategory(category.Id);
        var hasDeleted = new HasDeleted();

        if (!_sessionUser.IsInstallationAdmin &&
            _sessionUser.UserId != categoryCacheItem.Creator.Id)
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        if (!isTestCase)
        {
            await _categoryRelationRepo.DeleteByRelationIdAsync(category.Id)
                .ConfigureAwait(false);

            await _categoryRelationRepo.DeleteByCategoryIdAsync(category.Id)
                .ConfigureAwait(false);

            await _categoryRelationRepo.DeleteQuestionRelationsFromTopic(category.Id);
        }

        _userActivityRepo.DeleteForCategory(category.Id);

        _categoryChangeRepo.AddDeleteEntry(category, userId);
        _categoryValuationWritingRepo.DeleteCategoryValuation(category.Id);
        _categoryRepository.Delete(category);

        ModifyRelationsEntityCache.RemoveRelations(categoryCacheItem);
        EntityCache.Remove(categoryCacheItem, _permissionCheck, userId);
        _sessionUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        hasDeleted.DeletedSuccessful = true;
        return hasDeleted;
    }

    public record DeleteTopicResult(
        bool HasChildren,
        bool IsNotCreatorOrAdmin,
        bool Success,
        RedirectParent RedirectParent);

    public async Task<DeleteTopicResult> DeleteTopic(int id)
    {
        var redirectParent = GetRedirectTopic(id);
        var topic = categoryRepo.GetById(id);
        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        var parentIds =
            EntityCache.GetCategory(id).Parents().Select(c => c.Id)
                .ToList(); //if the parents are fetched directly from the category there is a problem with the flush
        var parentTopics = categoryRepo.GetByIds(parentIds);

        var hasDeleted = await Run(topic, _sessionUser.UserId);
        foreach (var parent in parentTopics)
        {
            _categoryChangeRepo.AddUpdateEntry(categoryRepo, parent, _sessionUser.UserId, false);
        }

        return new DeleteTopicResult(
            hasDeleted.HasChildren,
            hasDeleted.IsNotCreatorOrAdmin,
            hasDeleted.DeletedSuccessful,
            redirectParent
        );
    }

    public record RedirectParent(string Name, int Id);

    private RedirectParent GetRedirectTopic(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);
        var lastBreadcrumbItem =
            crumbtrailService.BuildCrumbtrail(topic, currentWiki).Items.LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectParent(lastBreadcrumbItem.Category.Name,
                lastBreadcrumbItem.Category.Id);

        return new RedirectParent(currentWiki.Name, currentWiki.Id);
    }

    private class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
    }
}
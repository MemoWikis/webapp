public class PageConversion(
    PermissionCheck _permissionCheck,
    PageRepository _pageRepository,
    PageRelationRepo _pageRelationRepo,
    UserWritingRepo _userWritingRepo) : IRegisterAsInstancePerLifetime
{
    public void ConvertPageToWiki(PageCacheItem page, int userId, bool keepParents = false)
    {
        if (userId < 1)
            throw new InvalidOperationException("Invalid user ID.");

        if (!_permissionCheck.CanConvertPage(page))
            throw new UnauthorizedAccessException("User does not have permission to convert the page to a Wiki.");

        page.IsWiki = true;

        if (!keepParents)
        {
            var modifyRelationsForPage = new ModifyRelationsForPage(_pageRepository, _pageRelationRepo);
            bool removed = ModifyRelationsEntityCache.RemoveAllParents(
                page,
                userId,
                modifyRelationsForPage,
                _permissionCheck);

            if (!removed)
                throw new InvalidOperationException("Failed to remove parent relations.");
        }

        EntityCache.AddOrUpdate(page);

        var pageEntity = _pageRepository.GetById(page.Id);
        if (pageEntity == null)
            throw new InvalidOperationException($"Page with ID {page.Id} not found in repository.");

        pageEntity.IsWiki = true;
        _pageRepository.Update(pageEntity);

        var user = EntityCache.GetUserById(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found in EntityCache.");

        AddWikiId(user, page.Id);
        user.CleanupWikiIdsAndFavoriteIds();
        EntityCache.AddOrUpdate(user);
        _userWritingRepo.Update(user);

        if (page.CreatorId != userId)
        {
            var creator = EntityCache.GetUserById(page.CreatorId);
            AddWikiId(creator, page.Id);
            creator.CleanupWikiIdsAndFavoriteIds();
            EntityCache.AddOrUpdate(creator);
            _userWritingRepo.Update(creator);

        }
    }

    private void AddWikiId(UserCacheItem user, int wikiId)
    {
        user.AddWiki(wikiId);
        EntityCache.AddOrUpdate(user);
        _userWritingRepo.Update(user);
    }

    public void ConvertWikiToPage(PageCacheItem page, int userId)
    {
        if (userId < 1)
            throw new InvalidOperationException("Invalid user ID.");

        if (!_permissionCheck.CanConvertPage(page))
            throw new UnauthorizedAccessException("User does not have permission to convert the Wiki to a page.");

        page.IsWiki = false;
        EntityCache.AddOrUpdate(page);

        var pageEntity = _pageRepository.GetById(page.Id);
        if (pageEntity == null)
            throw new InvalidOperationException($"Page with ID {page.Id} not found in repository.");

        pageEntity.IsWiki = false;
        _pageRepository.Update(pageEntity);

        var user = EntityCache.GetUserById(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found in EntityCache.");

        RemoveWikiId(user, page.Id);
        user.CleanupWikiIdsAndFavoriteIds();
        EntityCache.AddOrUpdate(user);

        if (page.CreatorId != userId)
        {
            var creator = EntityCache.GetUserById(page.CreatorId);
            RemoveWikiId(creator, page.Id);
            creator.CleanupWikiIdsAndFavoriteIds();
            EntityCache.AddOrUpdate(creator);
        }
    }

    private void RemoveWikiId(UserCacheItem user, int wikiId)
    {
        user.RemoveWiki(wikiId);
        EntityCache.AddOrUpdate(user);
        _userWritingRepo.Update(user);
    }
}
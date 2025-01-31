
public class PermissionCheck : IRegisterAsInstancePerLifetime
{
    private readonly int _userId;
    private readonly bool _isInstallationAdmin;

    public PermissionCheck(SessionUser sessionUser)
    {
        _userId = sessionUser.SessionIsActive() ? sessionUser.UserId : default;
        _isInstallationAdmin = sessionUser.SessionIsActive() && sessionUser.IsInstallationAdmin;
    }

    public PermissionCheck(UserCacheItem userCacheItem)
    {
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
    }

    public PermissionCheck(int userId)
    {
        var userCacheItem = EntityCache.GetUserById(userId);
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
    }

    //setter is for tests
    public bool CanViewPage(int id) => CanView(EntityCache.GetPage(id));
    public bool CanView(Page page) => CanView(EntityCache.GetPage(page.Id));
    public bool CanView(PageCacheItem? page) => CanView(_userId, page);

    public bool CanView(int userId, PageCacheItem? page)
    {
        if (page == null)
            return false;

        if (page.Visibility == PageVisibility.All)
            return true;

        if (page.Visibility == PageVisibility.Owner && page.CreatorId == userId)
            return true;

        return false;
    }

    public bool CanView(int creatorId, PageVisibility visibility)
    {
        if (visibility == PageVisibility.All)
            return true;

        if (visibility == PageVisibility.Owner && creatorId == _userId)
            return true;

        return false;
    }

    public bool CanEditPage(int paegId) => CanEdit(EntityCache.GetPage(paegId));
    public bool CanEdit(Page page) => CanEdit(EntityCache.GetPage(page.Id));

    public bool CanView(PageChange change)
    {
        return change.Page != null &&
               change.Page.Id > 0 &&
               CanView(change.Page) &&
               CanView(change.Page.Creator.Id, change.GetPageChangeData().Visibility);
    }

    public bool CanEdit(PageCacheItem page)
    {
        if (_userId == default)
            return false;

        if (page == null)
            return false;

        if (FeaturedPage.Lockedpage(page.Id) && !_isInstallationAdmin)
            return false;

        if (!CanView(page))
            return false;

        return true;
    }

    public bool CanConvertPage(PageCacheItem page)
    {
        return _isInstallationAdmin || _userId == page.CreatorId;
    }

    public bool CanDelete(PageCacheItem page)
    {
        if (_userId == default || page == null || page.Id == 0)
            return false;

        if (page.IsWikiType())
            return false;

        if (page.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanDelete(Page page)
    {
        if (_userId == default || page == null || page.Id == 0)
            return false;

        if (page.Id == FeaturedPage.RootPageId || page.Id == page.Creator.StartPageId)
            return false;

        if (page.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanMovePage(int pageId, int oldParentId, int newParentId) => CanMovePage(
        EntityCache.GetPage(pageId), EntityCache.GetPage(oldParentId), newParentId);

    public bool CanMovePage(PageCacheItem? movingPage, PageCacheItem? oldParent, int newParentId)
    {
        if (_userId == default
            || movingPage == null
            || movingPage.Id == 0
            || oldParent == null
            || oldParent.Id == 0)
            return false;

        if (FeaturedPage.RootPageId == newParentId && !_isInstallationAdmin && movingPage.Visibility == PageVisibility.All)
            return false;

        return _isInstallationAdmin || movingPage.CreatorId == _userId || oldParent.CreatorId == _userId;
    }

    public bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public bool CanView(QuestionCacheItem? question)
    {
        if (question == null || question.Id == 0)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.CreatorId == _userId)
            return true;

        return false;
    }

    public bool CanEditQuestion(int questionId) => CanEdit(EntityCache.GetQuestion(questionId));

    public bool CanEdit(QuestionCacheItem question)
    {
        if (_userId == default)
            return false;

        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanDelete(Question question)
    {
        if (_userId == default)
            return false;

        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return true;

        return false;
    }
}
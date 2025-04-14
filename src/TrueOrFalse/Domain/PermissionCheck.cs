
public class PermissionCheck : IRegisterAsInstancePerLifetime
{
    private readonly int _userId;
    private readonly bool _isInstallationAdmin;
    private readonly bool _isLoggedIn;
    private readonly Dictionary<int, string> _shareTokens = new Dictionary<int, string>();

    public PermissionCheck(SessionUser sessionUser)
    {
        _userId = sessionUser.SessionIsActive() ? sessionUser.UserId : default;
        _isInstallationAdmin = sessionUser.SessionIsActive() && sessionUser.IsInstallationAdmin;
        _isLoggedIn = sessionUser.SessionIsActive() && sessionUser.IsLoggedIn;
        _shareTokens = sessionUser.ShareTokens;
    }

    public PermissionCheck(UserCacheItem userCacheItem)
    {
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
        _isLoggedIn = false;
    }

    public PermissionCheck(int userId)
    {
        var userCacheItem = EntityCache.GetUserById(userId);
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
        _isLoggedIn = false;
    }

    //setter is for tests
    public bool CanViewPage(int id) => CanView(EntityCache.GetPage(id));
    public bool CanView(Page page) => CanView(EntityCache.GetPage(page.Id));
    public bool CanView(PageCacheItem? page) => CanView(_userId, page);

    public bool CanView(PageCacheItem? page, string shareToken) => CanView(_userId, page, shareToken);


    public bool CanView(int userId, PageCacheItem? page, string? token = null)
    {
        if (page == null)
            return false;

        if (page.Visibility == PageVisibility.All)
            return true;

        if (page.Visibility == PageVisibility.Owner && page.CreatorId == userId)
            return true;

        var shareInfos = EntityCache.GetPageShares(page.Id).Where(s => s.SharedWith?.Id == _userId);
        if (shareInfos.Any(s => s.Permission is SharePermission.RestrictAccess))
            return false;

        if (token != null || _shareTokens.Any())
        {
            var shareInfosByToken = EntityCache.GetPageShares(page.Id);
            _shareTokens.TryGetValue(page.Id, out var sessionUserToken);
            var shareByToken = shareInfosByToken.FirstOrDefault(share => share.Token == token || share.Token == sessionUserToken);
            if (shareByToken != null && shareByToken.Permission != SharePermission.RestrictAccess)
                return true;

            var closestSharePermissionByToken = SharesService.GetClosestParentSharePermissionByTokens(page.Id, _shareTokens);
            return closestSharePermissionByToken is SharePermission.EditWithChildren or SharePermission.ViewWithChildren;
        }

        if (shareInfos.Any(s => s.Permission is SharePermission.Edit
                or SharePermission.EditWithChildren
                or SharePermission.View
                or SharePermission.ViewWithChildren))
            return true;

        var closestSharePermission = SharesService.GetClosestParentSharePermissionByUserId(page.Id, _userId);
        return closestSharePermission is SharePermission.EditWithChildren or SharePermission.ViewWithChildren;
    }

    public bool CanView(int creatorId, PageVisibility visibility)
    {
        if (visibility == PageVisibility.All)
            return true;

        if (visibility == PageVisibility.Owner && creatorId == _userId)
            return true;

        return false;
    }
    public bool CanEditPage(int pageId, string? token, bool isLoggedIn = false) => CanEdit(EntityCache.GetPage(pageId), token, isLoggedIn);
    public bool CanEditPage(int pageId) => CanEdit(EntityCache.GetPage(pageId));
    public bool CanEdit(Page page) => CanEdit(EntityCache.GetPage(page.Id));

    public bool CanView(PageChange change)
    {
        return change.Page != null &&
               change.Page.Id > 0 &&
               CanView(change.Page) &&
               CanView(change.Page.Creator.Id, change.GetPageChangeData().Visibility);
    }

    public bool CanEdit(PageCacheItem page, string? token = null, bool isLoggedIn = false)
    {
        if (_userId == default)
            return false;

        if (page == null)
            return false;

        if (FeaturedPage.Lockedpage(page.Id) && !_isInstallationAdmin)
            return false;

        if (!CanView(page, token))
            return false;

        if (page.CreatorId == _userId || _isInstallationAdmin)
            return true;

        if (token != null && (_isLoggedIn || isLoggedIn))
        {
            var shareInfosByToken = EntityCache.GetPageShares(page.Id).Where(s => s.Token == token);
            if (shareInfosByToken.Any(s => s.Permission is SharePermission.Edit or SharePermission.EditWithChildren))
                return true;

            if (shareInfosByToken.Any(s => s.Permission is SharePermission.View or SharePermission.ViewWithChildren))
                return false;

            var closestSharePermissionByToken = SharesService.GetClosestParentSharePermissionByTokens(page.Id, _shareTokens);
            return closestSharePermissionByToken == SharePermission.EditWithChildren;
        }

        var shareInfos = EntityCache.GetPageShares(page.Id).Where(s => s.SharedWith?.Id == _userId);
        if (shareInfos.Any(s => s.Permission is SharePermission.Edit or SharePermission.EditWithChildren))
            return true;

        if (shareInfos.Any(s => s.Permission is SharePermission.View or SharePermission.ViewWithChildren))
            return false;

        var closestSharePermission = SharesService.GetClosestParentSharePermissionByUserId(page.Id, _userId);
        return closestSharePermission == SharePermission.EditWithChildren;
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
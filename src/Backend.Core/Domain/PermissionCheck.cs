public class PermissionCheck(ISessionUser _sessionUser) : IRegisterAsInstancePerLifetime
{
    public readonly record struct CanDeleteResult(bool Allowed, string? Reason);

    private int _userId => _sessionUser.SessionIsActive() ? _sessionUser.UserId : default;
    private bool _isInstallationAdmin => _sessionUser.SessionIsActive() && _sessionUser.IsInstallationAdmin;
    private bool _isLoggedIn => _sessionUser.SessionIsActive() && _sessionUser.IsLoggedIn;

    //setter is for tests
    public bool CanViewPage(int id) => CanView(EntityCache.GetPage(id));
    public bool CanView(Page page) => CanView(EntityCache.GetPage(page.Id));
    public bool CanView(PageCacheItem? page) => CanView(_userId, page);
    public bool CanViewPage(int id, string shareToken) => CanView(EntityCache.GetPage(id), shareToken);

    public bool CanView(PageCacheItem? page, string shareToken) => CanView(_userId, page, shareToken);

    public bool CanView(int userId, PageCacheItem? page, string? token = null)
    {
        if (page == null)
            return false;

        if (page.Visibility == PageVisibility.Public)
            return true;

        if (page.Visibility == PageVisibility.Private && page.CreatorId == userId)
            return true;

        var shareInfos = EntityCache.GetPageShares(page.Id).Where(s => s.SharedWith?.Id == _userId);
        if (shareInfos.Any(s => s.Permission is SharePermission.RestrictAccess))
            return false;

        if (token != null || (_sessionUser.ShareTokens != null && _sessionUser.ShareTokens.Any()))
        {
            var shareInfosByToken = EntityCache.GetPageShares(page.Id);
            _sessionUser.ShareTokens.TryGetValue(page.Id, out var sessionUserToken);
            var shareByToken =
                shareInfosByToken.FirstOrDefault(share => share.Token == token || share.Token == sessionUserToken);
            if (shareByToken != null && shareByToken.Permission != SharePermission.RestrictAccess)
                return true;

            var closestSharePermissionByToken =
                SharesService.GetClosestParentSharePermissionByTokens(page.Id, _sessionUser.ShareTokens);
            if (closestSharePermissionByToken != null)
                return closestSharePermissionByToken is SharePermission.EditWithChildren
                    or SharePermission.ViewWithChildren;
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
        if (visibility == PageVisibility.Public)
            return true;

        if (visibility == PageVisibility.Private && creatorId == _userId)
            return true;

        return false;
    }

    public bool CanEditPage(int pageId, string? token, bool isLoggedIn = false) =>
        CanEdit(EntityCache.GetPage(pageId), token, isLoggedIn);

    public bool CanEditPage(int pageId) => CanEdit(EntityCache.GetPage(pageId));
    public bool CanEdit(Page page) => CanEdit(EntityCache.GetPage(page.Id));

    public bool CanView(PageChange change)
    {
        return change.Page != null &&
               change.Page.Id > 0 &&
               CanView(change.Page) &&
               CanView(change.Page.Creator.Id, change.GetPageChangeData().Visibility);
    }

    public bool CanEdit(PageCacheItem? page, string? token = null, bool isLoggedIn = false)
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

        if (!(token != null || _sessionUser.ShareTokens.Any()) || !(_isLoggedIn || isLoggedIn))
        {
            var tokenPermission = TryGetEditPermissionByToken(page.Id, token);
            if (tokenPermission.HasValue)
                return tokenPermission.Value;
        }

        var shareInfos = EntityCache.GetPageShares(page.Id).Where(s => s.SharedWith?.Id == _userId);
        if (shareInfos.Any(s => s.Permission is SharePermission.Edit or SharePermission.EditWithChildren))
            return true;

        if (shareInfos.Any(s => s.Permission is SharePermission.View or SharePermission.ViewWithChildren))
            return false;

        var closestSharePermission = SharesService.GetClosestParentSharePermissionByUserId(page.Id, _userId);
        return closestSharePermission == SharePermission.EditWithChildren;
    }

    public bool? TryGetEditPermissionByToken(int pageId, string? token)
    {
        if (!(token != null || _sessionUser.ShareTokens.Any()))
            return null;

        var shareInfosByToken = EntityCache.GetPageShares(pageId);
        _sessionUser.ShareTokens.TryGetValue(pageId, out var sessionUserToken);

        var shareByToken =
            shareInfosByToken.FirstOrDefault(share => share.Token == token || share.Token == sessionUserToken);
        if (shareByToken != null)
        {
            if (shareByToken.Permission == SharePermission.RestrictAccess)
                return false;

            if (shareByToken.Permission == SharePermission.View ||
                shareByToken.Permission == SharePermission.ViewWithChildren)
                return false;

            if (shareByToken.Permission == SharePermission.Edit ||
                shareByToken.Permission == SharePermission.EditWithChildren)
                return true;
        }

        var closestSharePermissionByToken =
            SharesService.GetClosestParentSharePermissionByTokens(pageId, _sessionUser.ShareTokens);
        if (closestSharePermissionByToken != null)
            return closestSharePermissionByToken == SharePermission.EditWithChildren;

        return null;
    }

    public bool CanConvertPage(PageCacheItem page)
    {
        return _isInstallationAdmin || _userId == page.CreatorId;
    }

    public CanDeleteResult CanDelete(PageCacheItem page)
    {
        if (_userId == default || page == null || page.Id == 0)
            return new CanDeleteResult(false, FrontendMessageKeys.Error.Default);

        if (page.Creator.Id != _userId)
            return new CanDeleteResult(false, FrontendMessageKeys.Error.Page.NoRights);

        if (page.Id == FeaturedPage.RootPageId)
            return new CanDeleteResult(false, FrontendMessageKeys.Error.Page.NoRights);

        if (page.IsWiki)
        {
            var wouldHaveRemainingWiki = page.Creator.GetWikis().Count >= 2;
            if (wouldHaveRemainingWiki)
                return new CanDeleteResult(true, null);

            return new CanDeleteResult(false, FrontendMessageKeys.Error.User.NoRemainingWikis);
        }

        if (page.Creator.Id == _userId)
            return new CanDeleteResult(true, null);

        return new CanDeleteResult(false, FrontendMessageKeys.Error.Default);
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

        if (FeaturedPage.RootPageId == newParentId && !_isInstallationAdmin &&
            movingPage.Visibility == PageVisibility.Public)
            return false;

        return _isInstallationAdmin || movingPage.CreatorId == _userId || oldParent.CreatorId == _userId;
    }

    public bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public bool CanView(QuestionCacheItem? question)
    {
        if (question == null || question.Id == 0)
            return false;

        if (question.Visibility == QuestionVisibility.Public)
            return true;

        if (question.Visibility == QuestionVisibility.Private && question.CreatorId == _userId)
            return true;

        if (question.Pages?.Any() == true)
        {
            foreach (var page in question.Pages)
            {
                if (_sessionUser.ShareTokens != null)
                {
                    var shareInfosByToken = EntityCache.GetPageShares(page.Id);
                    _sessionUser.ShareTokens.TryGetValue(page.Id, out var sessionUserToken);
                    var shareByToken = shareInfosByToken.FirstOrDefault(share =>
                        share.Token == sessionUserToken &&
                        share.Permission != SharePermission.RestrictAccess);

                    if (shareByToken != null)
                        return true;

                    var closestSharePermissionByToken = SharesService.GetClosestParentSharePermissionByTokens(
                        page.Id, _sessionUser.ShareTokens);

                    if (closestSharePermissionByToken is SharePermission.EditWithChildren
                        or SharePermission.ViewWithChildren)
                        return true;
                }

                var shareInfos = EntityCache.GetPageShares(page.Id).Where(s => s.SharedWith?.Id == _userId);
                if (shareInfos.Any(s => s.Permission != SharePermission.RestrictAccess))
                    return true;

                var closestSharePermission = SharesService.GetClosestParentSharePermissionByUserId(page.Id, _userId);
                if (closestSharePermission is SharePermission.EditWithChildren
                    or SharePermission.ViewWithChildren)
                    return true;
            }
        }

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
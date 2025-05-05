using JetBrains.Annotations;

public class SharePageStoreController(
        PermissionCheck _permissionCheck,
        SessionUser _sessionUser,
        SharesRepository _sharesRepository,
        IHttpContextAccessor _httpContextAccessor,
        UserReadingRepo _userReadingRepo,
        NotifyUserPerEmail _notifyUserPerEmail
    ) : ApiBaseController
{
    public readonly record struct ShareToUserRequest(
        int PageId,
        int UserId,
        SharePermission Permission,
        [CanBeNull] string CustomMessage
    );

    public readonly record struct ShareToUserResponse(
        bool Success,
        string MessageKey
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public ShareToUserResponse ShareToUser([FromBody] ShareToUserRequest request)
    {
        var page = EntityCache.GetPage(request.PageId);
        if (page == null)
            return new ShareToUserResponse(false, FrontendMessageKeys.Error.Page.NotFound);

        if (!_permissionCheck.CanEdit(page))
            return new ShareToUserResponse(false, FrontendMessageKeys.Error.Page.MissingRights);

        SharesService.AddShareToPage(request.PageId, request.UserId, request.Permission, _sessionUser.UserId, _sharesRepository, _userReadingRepo);
        _notifyUserPerEmail.Run(request.UserId, request.PageId, request.Permission, request.CustomMessage);

        return new ShareToUserResponse(true, "");
    }

    public readonly record struct RenewShareTokenRequest(int PageId, [CanBeNull] string ShareToken);
    public readonly record struct RenewShareTokenResponse(
        bool Success,
        [CanBeNull] string Token = null

    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public RenewShareTokenResponse RenewShareToken([FromBody] RenewShareTokenRequest request)
    {
        var page = EntityCache.GetPage(request.PageId);
        if (page == null)
            return new RenewShareTokenResponse(false);

        if (!_permissionCheck.CanEdit(page, request.ShareToken))
            return new RenewShareTokenResponse(false);

        var token = SharesService.RenewShareToken(request.PageId, _sessionUser.UserId, _sharesRepository);

        return new RenewShareTokenResponse(true, token);
    }

    public readonly record struct SharePageByTokenRequest(int PageId, SharePermission Permission, [CanBeNull] string ShareToken);
    public readonly record struct SharePageByTokenResponse(
        bool Success,
        [CanBeNull] string Token
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SharePageByTokenResponse SharePageByToken([FromBody] SharePageByTokenRequest request)
    {
        var page = EntityCache.GetPage(request.PageId);
        if (page == null)
            return new SharePageByTokenResponse(false, null);
        if (!_permissionCheck.CanEdit(page, request.ShareToken))
            return new SharePageByTokenResponse(false, null);

        var token = SharesService.GetShareToken(request.PageId, request.Permission, _sessionUser.UserId, _sharesRepository);
        return new SharePageByTokenResponse(true, token);
    }

    public readonly record struct CreatorResponse(int Id, string Name, [CanBeNull] string ImageUrl);
    public readonly record struct UserWithPermission(
        int Id,
        string Name,
        [CanBeNull] string ImageUrl,
        SharePermission Permission,
        [CanBeNull] PageResponse? InheritedFrom = null);
    public readonly record struct GetShareInfoResponse(
        [CanBeNull] CreatorResponse? Creator,
        [CanBeNull] List<UserWithPermission> Users,
        [CanBeNull] string ShareToken = null,
        [CanBeNull] SharePermission? ShareTokenPermission = null,
        bool CanEdit = false);

    public readonly record struct PageResponse(int Id, string Name);

    [HttpGet]
    public GetShareInfoResponse GetShareInfo([FromRoute] int id, [CanBeNull] string token = null)
    {
        var page = EntityCache.GetPage(id);

        if (page == null)
            return new GetShareInfoResponse();

        if (!_permissionCheck.CanEdit(page, token))
            return new GetShareInfoResponse(null, null, ShareToken: token);

        var existingShares = EntityCache.GetPageShares(id);
        var users = existingShares.Where(s => s.SharedWith != null).Select(s =>
        {
            var imageUrl = new UserImageSettings(s.SharedWith.Id,
                    _httpContextAccessor)
                .GetUrl_50px_square(s.SharedWith)
                .Url;
            return new UserWithPermission(s.SharedWith.Id, s.SharedWith.Name, imageUrl, s.Permission);

        }).ToList();

        var existingUserIds = new HashSet<int>(users.Select(u => u.Id));
        var parentShares = SharesService.GetParentShareCacheItem(id);
        if (parentShares.Any())
        {
            var filteredParentShares = parentShares.Where(s =>
                s.SharedWith != null && !existingUserIds.Contains(s.SharedWith.Id));

            var usersWithParentPermissions = filteredParentShares.Select(s =>
            {
                var imageUrl = new UserImageSettings(s.SharedWith!.Id,
                        _httpContextAccessor)
                    .GetUrl_50px_square(s.SharedWith)
                    .Url;
                var parentPage = EntityCache.GetPage(s.PageId);
                var pageResponse = new PageResponse(parentPage!.Id, parentPage.Name);
                return new UserWithPermission(s.SharedWith.Id, s.SharedWith.Name, imageUrl, s.Permission, pageResponse);
            }).ToList();

            users.AddRange(usersWithParentPermissions);
        }

        var creator = EntityCache.GetUserById(page.CreatorId);
        var creatorResponse = new CreatorResponse(creator.Id, creator.Name, new UserImageSettings(
                creator.Id,
                _httpContextAccessor)
            .GetUrl_50px_square(creator)
            .Url);

        var shareByToken = existingShares.FirstOrDefault(s => s.Token.Length > 0);

        var filteredUsers = users.Where(u => u.Permission != SharePermission.RestrictAccess).ToList();

        return new GetShareInfoResponse(creatorResponse, filteredUsers, shareByToken?.Token, shareByToken?.Permission, _permissionCheck.CanEdit(page, token));
    }
    public readonly record struct PermissionUpdate(
        int UserId,
        SharePermission Permission
    );

    public readonly record struct BatchUpdatePermissionsResponse(
        bool Success,
        string MessageKey
    );
    public readonly record struct ParentRemoval(
        int UserId,
        int ParentPageId
    );

    public readonly record struct BatchUpdatePermissionsRequest(
        int PageId,
        List<PermissionUpdate> PermissionUpdates,
        List<int> RemovedUserIds,
        bool RemoveShareToken = false,
        SharePermission? TokenPermission = null,
        List<ParentRemoval> ParentRemovals = null);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public BatchUpdatePermissionsResponse BatchUpdatePermissions([FromBody] BatchUpdatePermissionsRequest request)
    {
        var page = EntityCache.GetPage(request.PageId);
        if (page == null)
            return new BatchUpdatePermissionsResponse(false, FrontendMessageKeys.Error.Page.NotFound);

        if (!_permissionCheck.CanEdit(page))
            return new BatchUpdatePermissionsResponse(false, FrontendMessageKeys.Error.Page.NoRights);

        // Process user permission updates and removals
        var permissionUpdates = request.PermissionUpdates
            .Select(p => (p.UserId, p.Permission))
            .ToList();

        var parentRemovals = request.ParentRemovals?
            .Select(p => (p.UserId, p.ParentPageId))
            .ToList();

        SharesService.BatchUpdatePageShares(
            request.PageId,
            permissionUpdates,
            request.RemovedUserIds,
            request.RemoveShareToken,
            request.TokenPermission,
            parentRemovals,
            _sessionUser.UserId,
            _sharesRepository,
            _userReadingRepo);

        return new BatchUpdatePermissionsResponse(true, "");
    }
}

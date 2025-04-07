using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;

public class SharePageStoreController(
        PermissionCheck _permissionCheck,
        SessionUser _sessionUser,
        ShareInfoRepository _shareInfoRepository,
        IHttpContextAccessor _httpContextAccessor
    ) : Controller
{
    public readonly record struct ShareInfoRequest(int UserId, SharePermission Permission);

    public readonly record struct EditRightsRequest(
        int PageId,
        List<ShareInfoRequest> Users
    );

    public readonly record struct EditRightsResponse(
        bool Success,
        string MessageKey
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public EditRightsResponse EditRights([FromBody] EditRightsRequest req)
    {
        var page = EntityCache.GetPage(req.PageId);
        if (page == null)
            return new EditRightsResponse(false, "Page not found.");

        if (!_permissionCheck.CanEdit(page))
            return new EditRightsResponse(false, "Missing rights to edit sharing settings for this page.");

        var shareInfos = req.Users.Select(u => new ShareInfoCacheItem
        {
            UserId = u.UserId,
            PageId = req.PageId,
            Permission = u.Permission,
            GrantedBy = _sessionUser.UserId,
            Token = ""
        }).ToList();

        EntityCache.AddOrUpdatePageShares(req.PageId, shareInfos);

        return new EditRightsResponse(true, "");
    }

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
    public ShareToUserResponse ShareToUser([FromBody] ShareToUserRequest req)
    {
        var page = EntityCache.GetPage(req.PageId);
        if (page == null)
            return new ShareToUserResponse(false, "Page not found.");

        if (!_permissionCheck.CanEdit(page))
            return new ShareToUserResponse(false, "Missing rights to share this page.");

        ShareInfoHelper.AddShareToPage(req.PageId, req.UserId, req.Permission, _sessionUser.UserId, _shareInfoRepository);

        return new ShareToUserResponse(true, "");
    }

    public readonly record struct RenewShareTokenRequest(int PageId);
    public readonly record struct RenewShareTokenResponse(
        bool Success,
        string MessageKey
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public RenewShareTokenResponse RenewShareToken([FromBody] RenewShareTokenRequest req)
    {
        var page = EntityCache.GetPage(req.PageId);
        if (page == null)
            return new RenewShareTokenResponse(false, "Page not found.");

        if (!_permissionCheck.CanEdit(page))
            return new RenewShareTokenResponse(false, "Missing rights to renew share token.");

        EntityCache.RenewTokenForPage(req.PageId);
        return new RenewShareTokenResponse(true, "");
    }

    public readonly record struct SharePageByTokenRequest(int PageId, SharePermission Permission);
    public readonly record struct SharePageByTokenResponse(
        bool Success,
        [CanBeNull] string Token
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SharePageByTokenResponse SharePageByToken([FromBody] SharePageByTokenRequest req)
    {
        var page = EntityCache.GetPage(req.PageId);
        if (page == null)
            return new SharePageByTokenResponse(false, null);
        if (!_permissionCheck.CanEdit(page))
            return new SharePageByTokenResponse(false, null);

        var token = ShareInfoHelper.GenerateShareToken(req.PageId, req.Permission, _sessionUser.UserId, _shareInfoRepository);
        return new SharePageByTokenResponse(true, token);
    }

    public readonly record struct UserWithPermission(
        int Id,
        string Name,
        [CanBeNull] string ImageUrl,
        SharePermission Permission);
    public readonly record struct GetShareInfoResponse([CanBeNull] List<UserWithPermission> Users);


    [HttpGet]
    public GetShareInfoResponse GetShareInfo([FromRoute] int id)
    {
        if (!_permissionCheck.CanEdit(EntityCache.GetPage(id)))
            return new GetShareInfoResponse();

        var existingShares = EntityCache.GetPageShares(id);
        var users = existingShares.Where(s => s.UserId != null).Select(s =>
        {
            var user = EntityCache.GetUserById((int)s.UserId);
            var imageUrl = new UserImageSettings(user.Id,
                    _httpContextAccessor)
                .GetUrl_50px_square(user)
                .Url;
            return new UserWithPermission(user.Id, user.Name, imageUrl, s.Permission);

        }).ToList();
        return new GetShareInfoResponse(users);
    }

    public readonly record struct BatchUpdatePermissionsRequest(
        int PageId,
        List<PermissionUpdate> PermissionUpdates,
        List<int> RemovedUserIds
    );

    public readonly record struct PermissionUpdate(
        int UserId,
        SharePermission Permission
    );

    public readonly record struct BatchUpdatePermissionsResponse(
        bool Success,
        string MessageKey
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public BatchUpdatePermissionsResponse BatchUpdatePermissions([FromBody] BatchUpdatePermissionsRequest req)
    {
        var page = EntityCache.GetPage(req.PageId);
        if (page == null)
            return new BatchUpdatePermissionsResponse(false, "Page not found.");

        if (!_permissionCheck.CanEdit(page))
            return new BatchUpdatePermissionsResponse(false, "Missing rights to update permissions for this page.");

        var existingShares = EntityCache.GetPageShares(req.PageId);

        foreach (var update in req.PermissionUpdates)
        {
            var share = existingShares.FirstOrDefault(s => s.UserId == update.UserId);
            if (share != null)
            {
                share.Permission = update.Permission;
                _shareInfoRepository.CreateOrUpdate(share.ToDbItem());
            }
            else
            {
                var newShare = new ShareInfo
                {
                    UserId = update.UserId,
                    PageId = req.PageId,
                    Permission = update.Permission,
                    GrantedBy = _sessionUser.UserId,
                    Token = ""
                };
                _shareInfoRepository.CreateOrUpdate(newShare);
                var dbItem = _shareInfoRepository.GetById(newShare.Id);
                var newShareCacheItem = ShareInfoCacheItem.ToCacheItem(dbItem);
                existingShares.Add(newShareCacheItem);
            }
        }

        foreach (var userId in req.RemovedUserIds)
        {
            var share = existingShares.FirstOrDefault(s => s.UserId == userId);
            if (share != null)
            {
                _shareInfoRepository.Delete(share.Id);
            }
        }
        existingShares.RemoveAll(s => s.UserId.HasValue && req.RemovedUserIds.Contains(s.UserId.Value));

        EntityCache.AddOrUpdatePageShares(req.PageId, existingShares);

        return new BatchUpdatePermissionsResponse(true, "");
    }

}

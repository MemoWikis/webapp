using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;

public class SharePageStoreController(
        PermissionCheck _permissionCheck,
        SessionUser _sessionUser,
        SharesRepository _sharesRepository,
        IHttpContextAccessor _httpContextAccessor,
        UserReadingRepo _userReadingRepo
    ) : Controller
{
    //public readonly record struct SharesRequest(int UserId, SharePermission Permission);

    //public readonly record struct EditRightsRequest(
    //    int PageId,
    //    List<SharesRequest> Users
    //);

    //public readonly record struct EditRightsResponse(
    //    bool Success,
    //    string MessageKey
    //);

    //[HttpPost]
    //[AccessOnlyAsLoggedIn]
    //public EditRightsResponse EditRights([FromBody] EditRightsRequest request)
    //{
    //    var page = EntityCache.GetPage(request.PageId);
    //    if (page == null)
    //        return new EditRightsResponse(false, "Page not found.");

    //    if (!_permissionCheck.CanEdit(page))
    //        return new EditRightsResponse(false, "Missing rights to edit sharing settings for this page.");

    //    var shareInfos = request.Users.Select(u => new ShareCacheItem
    //    {
    //        User = u.UserId,
    //        PageId = request.PageId,
    //        Permission = u.Permission,
    //        GrantedBy = _sessionUser.UserId,
    //        Token = ""
    //    }).ToList();

    //    EntityCache.AddOrUpdatePageShares(request.PageId, shareInfos);

    //    return new EditRightsResponse(true, "");
    //}

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
            return new ShareToUserResponse(false, FrontendMessageKeys.Error.Page.NotFound);

        if (!_permissionCheck.CanEdit(page))
            return new ShareToUserResponse(false, FrontendMessageKeys.Error.Page.MissingRights);

        SharesService.AddShareToPage(req.PageId, req.UserId, req.Permission, _sessionUser.UserId, _sharesRepository, _userReadingRepo);

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

    public readonly record struct UserWithPermission(
        int Id,
        string Name,
        [CanBeNull] string ImageUrl,
        SharePermission Permission);
    public readonly record struct GetShareInfoResponse([CanBeNull] List<UserWithPermission> Users, [CanBeNull] string ShareToken = null);


    [HttpGet]
    public GetShareInfoResponse GetShareInfo([FromRoute] int id, [CanBeNull] string token = null)
    {
        var page = EntityCache.GetPage(id);

        if (page != null && !_permissionCheck.CanEdit(page, token))
            return new GetShareInfoResponse();

        var existingShares = EntityCache.GetPageShares(id);
        var users = existingShares.Where(s => s.SharedWith != null).Select(s =>
        {
            var imageUrl = new UserImageSettings(s.SharedWith.Id,
                    _httpContextAccessor)
                .GetUrl_50px_square(s.SharedWith)
                .Url;
            return new UserWithPermission(s.SharedWith.Id, s.SharedWith.Name, imageUrl, s.Permission);

        }).ToList();

        var shareToken = existingShares.FirstOrDefault(s => s.Token.Length > 0)?.Token;
        return new GetShareInfoResponse(users, shareToken);
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
    public BatchUpdatePermissionsResponse BatchUpdatePermissions([FromBody] BatchUpdatePermissionsRequest request)
    {
        var page = EntityCache.GetPage(request.PageId);
        if (page == null)
            return new BatchUpdatePermissionsResponse(false, FrontendMessageKeys.Error.Page.NotFound);

        if (!_permissionCheck.CanEdit(page))
            return new BatchUpdatePermissionsResponse(false, FrontendMessageKeys.Error.Page.NoRights);

        var existingShares = EntityCache.GetPageShares(request.PageId);

        foreach (var update in request.PermissionUpdates)
        {
            var share = existingShares.FirstOrDefault(s => s.SharedWith?.Id == update.UserId);
            if (share != null)
            {
                share.Permission = update.Permission;
                _sharesRepository.CreateOrUpdate(share.ToDbItem(_userReadingRepo));
            }
            else
            {
                var newShare = new Share
                {
                    User = _userReadingRepo.GetById(update.UserId),
                    PageId = request.PageId,
                    Permission = update.Permission,
                    GrantedBy = _sessionUser.UserId,
                    Token = ""
                };
                _sharesRepository.CreateOrUpdate(newShare);
                var dbItem = _sharesRepository.GetById(newShare.Id);
                var newShareCacheItem = ShareCacheItem.ToCacheItem(dbItem);
                existingShares.Add(newShareCacheItem);
            }
        }

        foreach (var userId in request.RemovedUserIds)
        {
            var share = existingShares.FirstOrDefault(s => s.SharedWith?.Id == userId);
            if (share != null)
            {
                _sharesRepository.Delete(share.Id);
            }
        }
        existingShares.RemoveAll(s => s.SharedWith != null && request.RemovedUserIds.Contains(s.SharedWith.Id));

        EntityCache.AddOrUpdatePageShares(request.PageId, existingShares);

        return new BatchUpdatePermissionsResponse(true, "");
    }

}

using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;

public class SharePageStoreController(
        PermissionCheck _permissionCheck,
        SessionUser _sessionUser
    ) : Controller
{
    // This record remains for representing individual share settings.
    public readonly record struct ShareInfoRequest(int UserId, SharePermission Permission);

    // Renamed from SharePageRequest to EditRightsRequest for bulk editing.
    public readonly record struct EditRightsRequest(
        int PageId,
        List<ShareInfoRequest> Users
    );

    public readonly record struct EditRightsResponse(
        bool Success,
        string MessageKey
    );

    // Bulk editing endpoint (renamed from SharePage)
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

        // Replace the current sharing settings with the new bulk list.
        EntityCache.AddOrUpdatePageShares(req.PageId, shareInfos);

        return new EditRightsResponse(true, "");
    }

    // New endpoint to add or update sharing for a single user.
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

        // Get existing share settings for this page.
        var existingShares = EntityCache.GetPageShares(req.PageId);

        // Find if there's an existing entry for this user.
        var existingShare = existingShares.FirstOrDefault(s => s.UserId == req.UserId);
        if (existingShare != null)
        {
            // Update the permission.
            existingShare.Permission = req.Permission;
        }
        else
        {
            // Add new share info.
            existingShares.Add(new ShareInfoCacheItem
            {
                UserId = req.UserId,
                PageId = req.PageId,
                Permission = req.Permission,
                GrantedBy = _sessionUser.UserId,
                Token = ""
            });
        }

        // Update the in-memory cache.
        EntityCache.AddOrUpdatePageShares(req.PageId, existingShares);

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

    public readonly record struct SharePageByTokenRequest(int PageId);
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

        // Generate a new token and store it in the cache.
        var token = ShareInfoHelper.GenerateShareToken(req.PageId, _sessionUser.UserId);
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
            var imageUrl = "";
            return new UserWithPermission(user.Id, user.Name, imageUrl, s.Permission);

        }).ToList();
        return new GetShareInfoResponse(users);
    }
}

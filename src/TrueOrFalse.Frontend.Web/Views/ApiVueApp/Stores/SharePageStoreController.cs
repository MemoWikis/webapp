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
    public readonly record struct SharePageRequest(
        int PageId,
        List<ShareInfoRequest> Users);

    public readonly record struct ShareInfoRequest(int UserId, SharePermission Permission);

    public readonly record struct SharePageResponse(
        bool Success,
        string MessageKey
    );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SharePageResponse SharePage([FromBody] SharePageRequest req)
    {
        var page = EntityCache.GetPage(req.PageId);
        if (page == null)
            return new SharePageResponse(false, "Page not found.");

        if (!_permissionCheck.CanEdit(page))
            return new SharePageResponse(false, "Missing rights to share this page.");

        var shareInfos = req.Users.Select(u => new ShareInfoCacheItem
        {
            UserId = u.UserId,
            PageId = req.PageId,
            Permission = u.Permission,
            GrantedBy = _sessionUser.UserId,
            Token = ""
        }).ToList();

        EntityCache.AddOrUpdate(req.PageId, shareInfos);

        return new SharePageResponse(true, "");
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
}


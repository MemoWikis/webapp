public class PageUpdater(
    PageRepository pageRepository,
    PermissionCheck permissionCheck) : IRegisterAsInstancePerLifetime
{
    public bool HideOrShowPageText(bool hideText, int pageId)
    {
        var cachePage = EntityCache.GetPage(pageId);
        if (cachePage == null)
            throw new NullReferenceException($"{nameof(HideOrShowPageText)}: pageCacheItem is null");

        //if (cachePage.Content?.Length > 0)
        //    throw new AccessViolationException($"{nameof(HideOrShowPageText)}: pageCacheItem has content");

        if (permissionCheck.CanView(cachePage) == false)
            throw new AccessViolationException($"{nameof(HideOrShowPageText)}: No permission for user");

        cachePage.TextIsHidden = hideText;
        EntityCache.AddOrUpdate(cachePage);

        var dbPage = pageRepository.GetById(pageId);
        dbPage.TextIsHidden = hideText;
        pageRepository.BaseUpdate(dbPage);

        return cachePage.TextIsHidden;
    }
}


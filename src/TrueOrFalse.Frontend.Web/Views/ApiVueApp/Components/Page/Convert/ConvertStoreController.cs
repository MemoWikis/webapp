using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ConvertStoreController
(
    SessionUser _sessionUser, PermissionCheck _permissionCheck, PageRepository _pageRepository) : Controller
{
    [HttpGet]
    public GetConvertDataResult GetConvertData([FromRoute] int id)
    {
        if (!_permissionCheck.CanViewPage(id))
            return new GetConvertDataResult(false, "", FrontendMessageKeys.Error.Page.MissingRights);

        var page = EntityCache.GetPage(id);

        return new GetConvertDataResult(page.IsWiki, page.Name);
    }
    public record struct GetConvertDataResult(bool IsWiki, string Name, string? MessageKey = null);

    [HttpPost]
    public ConversionResult ConvertPageToWiki([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);

        if (page == null)
            return new ConversionResult(false, FrontendMessageKeys.Error.Default);


        if (!_permissionCheck.CanConvertPage(page))
            return new ConversionResult(false, FrontendMessageKeys.Error.Page.MissingRights);

        page.IsWiki = true;
        EntityCache.AddOrUpdate(page);

        var pageEntity = _pageRepository.GetByIdEager(id);
        pageEntity.IsWiki = true;
        _pageRepository.Update(pageEntity);

        _sessionUser.User.CleanupWikiIdsAndFavoriteIds();

        return new ConversionResult(true);
    }


    [HttpPost]
    public ConversionResult ConvertWkiToPage([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);

        if (page == null)
            return new ConversionResult(false, FrontendMessageKeys.Error.Default);

        if (!_permissionCheck.CanConvertPage(page))
            return new ConversionResult(false, FrontendMessageKeys.Error.Page.MissingRights);

        page.IsWiki = false;
        EntityCache.AddOrUpdate(page);

        var pageEntity = _pageRepository.GetByIdEager(id);
        pageEntity.IsWiki = false;
        _pageRepository.Update(pageEntity);

        _sessionUser.User.CleanupWikiIdsAndFavoriteIds();

        return new ConversionResult(true);
    }

    public record struct ConversionResult(bool Success, string? MessageKey = null);

}
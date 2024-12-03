using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ConvertStoreController
(
    SessionUser _sessionUser, PermissionCheck _permissionCheck, PageRepository _pageRepository, PageRelationRepo _pageRelationRepo) : Controller
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
    public ConversionResponse ConvertPageToWiki([FromBody] ConvertPageToWikiRequest req)
    {
        var page = EntityCache.GetPage(req.Id);

        if (page == null)
            return new ConversionResponse(false, FrontendMessageKeys.Error.Default);


        if (!_permissionCheck.CanConvertPage(page))
            return new ConversionResponse(false, FrontendMessageKeys.Error.Page.MissingRights);


        page.IsWiki = true;

        if (!req.KeepParents)
        {
            var modifyRelationsForPage = new ModifyRelationsForPage(_pageRepository, _pageRelationRepo);
            ModifyRelationsEntityCache.RemoveAllParents(
                page,
                _sessionUser.UserId,
                modifyRelationsForPage,
                _permissionCheck);
        }

        EntityCache.AddOrUpdate(page);

        var pageEntity = _pageRepository.GetByIdEager(req.Id);
        pageEntity.IsWiki = true;
        _pageRepository.Update(pageEntity);

        _sessionUser.User.CleanupWikiIdsAndFavoriteIds();

        return new ConversionResponse(true);
    }

    public readonly record struct ConvertPageToWikiRequest(int Id, bool KeepParents = false);


    [HttpPost]
    public ConversionResponse ConvertWkiToPage([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);

        if (page == null)
            return new ConversionResponse(false, FrontendMessageKeys.Error.Default);

        if (!_permissionCheck.CanConvertPage(page))
            return new ConversionResponse(false, FrontendMessageKeys.Error.Page.MissingRights);

        page.IsWiki = false;
        EntityCache.AddOrUpdate(page);

        var pageEntity = _pageRepository.GetByIdEager(id);
        pageEntity.IsWiki = false;
        _pageRepository.Update(pageEntity);

        _sessionUser.User.CleanupWikiIdsAndFavoriteIds();

        return new ConversionResponse(true);
    }

    public record struct ConversionResponse(bool Success, string? MessageKey = null);

}
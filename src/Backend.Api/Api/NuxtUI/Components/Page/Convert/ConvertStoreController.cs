public class ConvertStoreController(
    SessionUser _sessionUser, 
    PermissionCheck _permissionCheck, 
    PageConversion _pageConversion) : ApiBaseController
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

        _pageConversion.ConvertPageToWiki(page, _sessionUser.UserId, req.KeepParents);

        return new ConversionResponse(true);
    }

    public readonly record struct ConvertPageToWikiRequest(int Id, bool KeepParents = false);


    [HttpPost]
    public ConversionResponse ConvertWikiToPage([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);

        if (page == null)
            return new ConversionResponse(false, FrontendMessageKeys.Error.Default);

        if (!_permissionCheck.CanConvertPage(page))
            return new ConversionResponse(false, FrontendMessageKeys.Error.Page.MissingRights);

        _pageConversion.ConvertWikiToPage(page, _sessionUser.UserId);

        return new ConversionResponse(true);
    }

    public record struct ConversionResponse(bool Success, string? MessageKey = null);

}
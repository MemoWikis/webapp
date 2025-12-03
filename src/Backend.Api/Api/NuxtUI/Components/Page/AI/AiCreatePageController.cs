public class AiCreatePageController(
    SessionUser _sessionUser,
    PageCreator _pageCreator,
    AiPageGenerator _aiPageGenerator,
    PageRepository _pageRepository,
    PermissionCheck _permissionCheck) : ApiBaseController
{
    public readonly record struct GenerateRequest(
        string Prompt,
        int DifficultyLevel,
        int ContentLength,
        int ParentId);

    public readonly record struct GenerateFromUrlRequest(
        string Url,
        int DifficultyLevel,
        int ContentLength,
        int ParentId);

    public readonly record struct GeneratedPageData(
        string Title,
        string HtmlContent);

    public readonly record struct GenerateResponse(
        bool Success,
        GeneratedPageData? Data = null,
        string MessageKey = "");

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<GenerateResponse> Generate([FromBody] GenerateRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        // Check if user can edit the parent page (if specified)
        if (request.ParentId > 0 && !_permissionCheck.CanEditPage(request.ParentId))
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Page.NoRights);
        }

        var difficultyLevel = (AiPageGenerator.DifficultyLevel)request.DifficultyLevel;
        var contentLength = (AiPageGenerator.ContentLength)request.ContentLength;
        
        var result = await _aiPageGenerator.Generate(
            request.Prompt,
            difficultyLevel,
            contentLength,
            _sessionUser.UserId,
            request.ParentId);

        if (result == null)
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        return new GenerateResponse(
            true,
            new GeneratedPageData(result.Value.Title, result.Value.HtmlContent));
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<GenerateResponse> GenerateFromUrl([FromBody] GenerateFromUrlRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (string.IsNullOrWhiteSpace(request.Url))
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        // Validate URL format
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Ai.InvalidUrl);
        }

        // Check if user can edit the parent page (if specified)
        if (request.ParentId > 0 && !_permissionCheck.CanEditPage(request.ParentId))
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Page.NoRights);
        }

        var difficultyLevel = (AiPageGenerator.DifficultyLevel)request.DifficultyLevel;
        var contentLength = (AiPageGenerator.ContentLength)request.ContentLength;
        
        var result = await _aiPageGenerator.GenerateFromUrl(
            request.Url,
            difficultyLevel,
            contentLength,
            _sessionUser.UserId,
            request.ParentId);

        if (result == null)
        {
            return new GenerateResponse(false, null, FrontendMessageKeys.Error.Ai.UrlFetchFailed);
        }

        return new GenerateResponse(
            true,
            new GeneratedPageData(result.Value.Title, result.Value.HtmlContent));
    }

    public readonly record struct CreateRequest(
        string Title,
        string HtmlContent,
        int ParentId,
        bool IsWiki = false);

    public readonly record struct CreateResponse(
        bool Success,
        int? PageId = null,
        string MessageKey = "");

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateResponse Create([FromBody] CreateRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new CreateResponse(false, null, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return new CreateResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        // Check if user can edit the parent page (if specified)
        if (request.ParentId > 0 && !_permissionCheck.CanEditPage(request.ParentId))
        {
            return new CreateResponse(false, null, FrontendMessageKeys.Error.Page.NoRights);
        }

        // Check if user can create a private page
        var limitCheck = new LimitCheck(_sessionUser);
        if (!limitCheck.CanSavePrivatePage(true))
        {
            return new CreateResponse(false, null, FrontendMessageKeys.Error.Subscription.CantSavePrivatePage);
        }

        // Create the page using the existing PageCreator
        var createResult = _pageCreator.Create(request.Title, request.ParentId, _sessionUser);
        
        if (!createResult.Success)
        {
            return new CreateResponse(false, null, createResult.MessageKey);
        }

        // Update the page content with the generated HTML
        var pageCacheItem = EntityCache.GetPage(createResult.Data.Id);
        var page = _pageRepository.GetByIdEager(createResult.Data.Id);
        
        if (pageCacheItem != null && page != null)
        {
            pageCacheItem.Content = request.HtmlContent;
            page.Content = request.HtmlContent;
            
            // Set as wiki if requested
            if (request.IsWiki)
            {
                pageCacheItem.IsWiki = true;
                page.IsWiki = true;
            }
            
            EntityCache.AddOrUpdate(pageCacheItem);
            _pageRepository.Update(page, _sessionUser.UserId, type: PageChangeType.Text);
        }

        return new CreateResponse(true, createResult.Data.Id);
    }
}

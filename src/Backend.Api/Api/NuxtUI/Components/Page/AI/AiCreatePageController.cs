public class AiCreatePageController(
    SessionUser _sessionUser,
    PageCreator _pageCreator,
    AiPageGenerator _aiPageGenerator,
    PageRepository _pageRepository,
    PermissionCheck _permissionCheck,
    AiModelRegistry _aiModelRegistry) : ApiBaseController
{
    public readonly record struct AiModelItem(
        string ModelId,
        string DisplayName,
        string Provider,
        decimal TokenCostMultiplier);

    public readonly record struct GetModelsResponse(
        bool Success,
        List<AiModelItem> Models);

    [HttpGet]
    public GetModelsResponse GetModels()
    {
        // Uses cache if available, falls back to DB
        var enabledModels = _aiModelRegistry.GetEnabledModels();

        var models = enabledModels
            .Select(model => new AiModelItem(
                model.ModelId,
                model.DisplayName,
                model.Provider.ToString(),
                model.TokenCostMultiplier))
            .ToList();

        return new GetModelsResponse(true, models);
    }

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

    public readonly record struct GenerateWikiRequest(
        string Prompt,
        int DifficultyLevel,
        int ParentId);

    public readonly record struct GenerateWikiFromUrlRequest(
        string Url,
        int DifficultyLevel,
        int ParentId);

    public readonly record struct GeneratedPageData(
        string Title,
        string HtmlContent);

    public readonly record struct GeneratedSubpageData(
        string Title,
        string HtmlContent);

    public readonly record struct GeneratedWikiData(
        string Title,
        string HtmlContent,
        List<GeneratedSubpageData> Subpages);

    public readonly record struct GenerateResponse(
        bool Success,
        GeneratedPageData? Data = null,
        string MessageKey = "");

    public readonly record struct GenerateWikiResponse(
        bool Success,
        GeneratedWikiData? Data = null,
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

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<GenerateWikiResponse> GenerateWiki([FromBody] GenerateWikiRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        // Check if user can edit the parent page (if specified)
        if (request.ParentId > 0 && !_permissionCheck.CanEditPage(request.ParentId))
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Page.NoRights);
        }

        var difficultyLevel = (AiPageGenerator.DifficultyLevel)request.DifficultyLevel;

        var result = await _aiPageGenerator.GenerateWikiWithSubpages(
            request.Prompt,
            difficultyLevel,
            _sessionUser.UserId,
            request.ParentId);

        if (result == null)
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        var subpages = result.Value.Subpages
            .Select(subpage => new GeneratedSubpageData(subpage.Title, subpage.HtmlContent))
            .ToList();

        return new GenerateWikiResponse(
            true,
            new GeneratedWikiData(result.Value.Title, result.Value.HtmlContent, subpages));
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<GenerateWikiResponse> GenerateWikiFromUrl([FromBody] GenerateWikiFromUrlRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (string.IsNullOrWhiteSpace(request.Url))
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Default);
        }

        // Validate URL format
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Ai.InvalidUrl);
        }

        // Check if user can edit the parent page (if specified)
        if (request.ParentId > 0 && !_permissionCheck.CanEditPage(request.ParentId))
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Page.NoRights);
        }

        var difficultyLevel = (AiPageGenerator.DifficultyLevel)request.DifficultyLevel;

        var result = await _aiPageGenerator.GenerateWikiWithSubpagesFromUrl(
            request.Url,
            difficultyLevel,
            _sessionUser.UserId,
            request.ParentId);

        if (result == null)
        {
            return new GenerateWikiResponse(false, null, FrontendMessageKeys.Error.Ai.UrlFetchFailed);
        }

        var subpages = result.Value.Subpages
            .Select(subpage => new GeneratedSubpageData(subpage.Title, subpage.HtmlContent))
            .ToList();

        return new GenerateWikiResponse(
            true,
            new GeneratedWikiData(result.Value.Title, result.Value.HtmlContent, subpages));
    }

    public readonly record struct CreateWikiRequest(
        string Title,
        string HtmlContent,
        List<CreateSubpageRequest> Subpages,
        int ParentId);

    public readonly record struct CreateSubpageRequest(
        string Title,
        string HtmlContent);

    public readonly record struct CreateWikiResponse(
        bool Success,
        int? WikiId = null,
        List<int>? SubpageIds = null,
        string MessageKey = "");

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateWikiResponse CreateWiki([FromBody] CreateWikiRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new CreateWikiResponse(false, null, null, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return new CreateWikiResponse(false, null, null, FrontendMessageKeys.Error.Default);
        }

        // Check if user can edit the parent page (if specified)
        if (request.ParentId > 0 && !_permissionCheck.CanEditPage(request.ParentId))
        {
            return new CreateWikiResponse(false, null, null, FrontendMessageKeys.Error.Page.NoRights);
        }

        // Check if user can create private pages (wiki + subpages)
        var limitCheck = new LimitCheck(_sessionUser);
        if (!limitCheck.CanSavePrivatePage(true))
        {
            return new CreateWikiResponse(false, null, null, FrontendMessageKeys.Error.Subscription.CantSavePrivatePage);
        }

        // Create the wiki
        var wikiResult = _pageCreator.Create(request.Title, request.ParentId, _sessionUser);

        if (!wikiResult.Success)
        {
            return new CreateWikiResponse(false, null, null, wikiResult.MessageKey);
        }

        var wikiId = wikiResult.Data.Id;
        var wikiCacheItem = EntityCache.GetPage(wikiId);
        var wikiPage = _pageRepository.GetByIdEager(wikiId);

        if (wikiCacheItem != null && wikiPage != null)
        {
            wikiCacheItem.Content = request.HtmlContent;
            wikiCacheItem.IsWiki = true;
            wikiPage.Content = request.HtmlContent;
            wikiPage.IsWiki = true;

            EntityCache.AddOrUpdate(wikiCacheItem);
            _pageRepository.Update(wikiPage, _sessionUser.UserId, type: PageChangeType.Text);
        }

        // Create subpages
        var subpageIds = new List<int>();
        foreach (var subpage in request.Subpages)
        {
            var subpageResult = _pageCreator.Create(subpage.Title, wikiId, _sessionUser);

            if (subpageResult.Success)
            {
                var subpageId = subpageResult.Data.Id;
                var subpageCacheItem = EntityCache.GetPage(subpageId);
                var subpagePage = _pageRepository.GetByIdEager(subpageId);

                if (subpageCacheItem != null && subpagePage != null)
                {
                    subpageCacheItem.Content = subpage.HtmlContent;
                    subpagePage.Content = subpage.HtmlContent;

                    EntityCache.AddOrUpdate(subpageCacheItem);
                    _pageRepository.Update(subpagePage, _sessionUser.UserId, type: PageChangeType.Text);
                }

                subpageIds.Add(subpageId);
            }
        }

        return new CreateWikiResponse(true, wikiId, subpageIds);
    }
}
